using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
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




/*private ExaminationDTO? CreateExaminationDTOFromInputData(Doctor doctor)
        {
            DateTime appointment = (DateTime)datePicker.SelectedDate;
            int minutes = int.Parse(minuteComboBox.Text);
            int hours = int.Parse(hourComboBox.Text);
            appointment = appointment.AddHours(hours);
            appointment = appointment.AddMinutes(minutes);
            ExaminationDTO examination = new ExaminationDTO(appointment, null, doctor, _medicalRecord);
            if (examination.Appointment <= DateTime.Now)
            {
                System.Windows.MessageBox.Show("You have to change dates for upcoming ones!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            else
                return examination;
        }

        private void ScheduleWithSpecificDoctor1()
        {
            try
            {
                ExaminationDTO? examination = CreateExaminationDTOFromInputData(_referral.ReferredDoctor);
                if(examination!=null)
                {
                    ExaminationRepository.GetInstance().ReserveExamination(examination);
                    System.Windows.MessageBox.Show("You have scheduled the examination!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    _referral.Active = false;
                    ReferralRepository.GetInstance().Save(); //TODO
                    Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ScheduleWithOrderedSpecialist1(Doctor doctor)
        {
            try
            {
                ExaminationDTO? examination = CreateExaminationDTOFromInputData(doctor);
                if (examination!=null)
                {
                    ExaminationRepository.GetInstance().ReserveExamination(examination);
                    System.Windows.MessageBox.Show("You have scheduled the examination! Doctor: " + doctor.Name + " " + doctor.Surname, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    _referral.Active = false;
                    ReferralRepository.GetInstance().Save(); //TODO
                    Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "That doctor is not available")
                    throw new Exception("That doctor is not available");
                else
                    System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        void ScheduleWithOrderedSpecialty1() 
        {
            List<Doctor> doctors = DoctorRepository.GetInstance().Doctors;
            foreach (Doctor doctor in doctors)
            {
                if (doctor.Specialty == _referral.ReferredSpecialty)
                {
                    try
                    {
                        ScheduleWithOrderedSpecialist(doctor);
                        break;
                    }
                    catch
                    {
                        continue;
                    }

                }
            }
        }*/



