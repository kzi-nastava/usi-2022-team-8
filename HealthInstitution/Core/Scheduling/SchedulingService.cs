using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Rooms.Model;
using System.Windows;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Examinations;

namespace HealthInstitution.Core.Scheduling
{
    internal static class SchedulingService
    {

        private static ExaminationRepository s_examinationRepository = ExaminationRepository.GetInstance();
        static OperationRepository s_operationRepository = OperationRepository.GetInstance();
        
        //hm da li ova treba u schedule service
        public static bool CheckOccurrenceOfRoom(Room room)
        {
            if (s_examinationRepository.Examinations.Find(examination => examination.Room == room) == null)
            {
                return false;
            }

            if (s_operationRepository.Operations.Find(operation => operation.Room == room) == null)
            {
                return false;
            }
            return true;
        }
        public static void RedirectByType(Referral referral, DateTime appointment, MedicalRecord medicalRecord)
        {
            if (referral.Type == ReferralType.SpecificDoctor)
                ScheduleWithSpecificDoctor(appointment, referral, medicalRecord);
            else
                ScheduleWithOrderedSpecialty(appointment, referral, medicalRecord);
        }
        private static void ScheduleWithSpecificDoctor(DateTime appointment, Referral referral, MedicalRecord medicalRecord)
        {
            ExaminationDTO examination = new ExaminationDTO(appointment, null, referral.ReferredDoctor, medicalRecord);
            ReserveExamination(examination);
            System.Windows.MessageBox.Show("You have scheduled the examination!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

            referral.Active = false;
            ReferralRepository.GetInstance().Save(); //TODO

        }
        private static void ScheduleWithOrderedSpecialist(ExaminationDTO examination, Referral referral)
        {
            try
            {
                ReserveExamination(examination);
                System.Windows.MessageBox.Show("You have scheduled the examination! Doctor: " + examination.Doctor.Name + " " + examination.Doctor.Surname, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                referral.Active = false;
                ReferralRepository.GetInstance().Save(); //TODO
            }
            catch (Exception ex)
            {
                if (ex.Message == "That doctor is not available")
                    throw new Exception("That doctor is not available");
                else
                    System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private static void ScheduleWithOrderedSpecialty(DateTime appointment, Referral referral, MedicalRecord medicalRecord)
        {
            SpecialtyType specialtyType = (SpecialtyType)referral.Type;
            List<Doctor> doctors = DoctorRepository.GetInstance().Doctors;
            bool successfulScheduling = false;
            foreach (Doctor doctor in doctors)
            {
                if (doctor.Specialty == specialtyType)
                {
                    try
                    {
                        ExaminationDTO examinationDTO = new ExaminationDTO(appointment, null, doctor, medicalRecord);
                        ScheduleWithOrderedSpecialist(examinationDTO, referral);
                        successfulScheduling = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "That doctor is not available")
                            continue;
                        throw;
                    }

                }
            }
            if (!successfulScheduling) throw new Exception("It is not possible to find a doctor with this specialty");
        }
        public static void ReserveOperation(OperationDTO operationDTO, int id = 0)
        {
            operationDTO.Room = RoomService.FindAvailableRoom(operationDTO);
            DoctorOperationAvailabilityService.CheckIfDoctorIsAvailable(operationDTO);
            PatientOperationAvailabilityService.CheckIfPatientIsAvailable(operationDTO);
            OperationService.Add(operationDTO);
        }

        public static void ReserveExamination(ExaminationDTO examinationDTO)
        {
            examinationDTO.Room = RoomService.FindAvailableRoom(examinationDTO);
            DoctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
            PatientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
            ExaminationService.Add(examinationDTO);
        }
    }
}
