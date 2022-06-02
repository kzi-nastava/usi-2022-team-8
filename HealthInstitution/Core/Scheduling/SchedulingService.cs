using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Core.Scheduling
{
    internal static class SchedulingService
    {
        //koristiti urgentService
        private static ExaminationRepository s_examinationRepository = ExaminationRepository.GetInstance();
        private static OperationRepository s_operationRepository = OperationRepository.GetInstance();

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
            ExaminationRepository.GetInstance().ReserveExamination(examination);
            System.Windows.MessageBox.Show("You have scheduled the examination!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);


            referral.Active = false;
            ReferralRepository.GetInstance().Save(); //TODO

        }
        private static void ScheduleWithOrderedSpecialist(ExaminationDTO examination, Referral referral)
        {
            try
            {
                ExaminationRepository.GetInstance().ReserveExamination(examination);
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
    }
}
