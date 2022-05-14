using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthInstitution.GUI.SecretaryView
{
    /// <summary>
    /// Interaction logic for AddExaminationWithReferralDialog.xaml
    /// </summary>
    public partial class AddExaminationWithReferralDialog : Window
    {
        Referral _referral;
        MedicalRecord _medicalRecord;
        public AddExaminationWithReferralDialog(Referral referral, MedicalRecord medicalRecord)
        {
            InitializeComponent();
            _referral = referral;
            _medicalRecord = medicalRecord;
            doctorBox.Text= (referral.ReferredDoctor==null) ? "" : referral.ReferredDoctor.Name + " " + referral.ReferredDoctor.Surname;
            specialtyBox.Text = (referral.ReferredSpecialty == null) ? "" : referral.ReferredSpecialty.ToString();
            patientBox.Text = medicalRecord.Patient.Name + " " + medicalRecord.Patient.Surname;
        }

        private void HourComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var hourComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> hours = new List<String>();
            for (int i = 9; i < 22; i++)
            {
                hours.Add(i.ToString());
            }
            hourComboBox.ItemsSource = hours;
            hourComboBox.SelectedIndex = 0;
        }

        private void MinuteComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> minutes = new List<String>();
            minutes.Add("00");
            minutes.Add("15");
            minutes.Add("30");
            minutes.Add("45");
            minuteComboBox.ItemsSource = minutes;
            minuteComboBox.SelectedIndex = 0;
        }
        private ExaminationDTO CreateExaminationDTOFromInputData(Doctor doctor)
        {
            DateTime appointment = (DateTime)datePicker.SelectedDate;
            int minutes = Int32.Parse(minuteComboBox.Text);
            int hours = Int32.Parse(hourComboBox.Text);
            appointment = appointment.AddHours(hours);
            appointment = appointment.AddMinutes(minutes);
            ExaminationDTO examination = new ExaminationDTO(appointment, null, doctor, _medicalRecord);
            return examination;
        }
        private void ScheduleWithSpecificDoctor()
        {
            try
            {
                ExaminationDTO examination = CreateExaminationDTOFromInputData(_referral.ReferredDoctor);
                if (examination.Appointment <= DateTime.Now)
                    System.Windows.MessageBox.Show("You have to change dates for upcoming ones!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    ExaminationRepository.GetInstance().ReserveExamination(examination);
                    System.Windows.MessageBox.Show("You have scheduled the examination!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    _referral.Active = false;
                    ReferralRepository.GetInstance().Save();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ScheduleWithOrderedSpecialist(Doctor doctor)
        {
            try
            {
                ExaminationDTO examination = CreateExaminationDTOFromInputData(doctor);
                if (examination.Appointment <= DateTime.Now)
                    System.Windows.MessageBox.Show("You have to change dates for upcoming ones!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    ExaminationRepository.GetInstance().ReserveExamination(examination);
                    System.Windows.MessageBox.Show("You have scheduled the examination! Doctor: " + doctor.Name + " " + doctor.Surname, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    _referral.Active = false;
                    ReferralRepository.GetInstance().Save();
                    this.Close();
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

        void ScheduleWithOrderedSpecialty()
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
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (_referral.Type == ReferralType.SpecificDoctor)
            {
                ScheduleWithSpecificDoctor();
            }
            else
            {
                ScheduleWithOrderedSpecialty();
            }
        }
    }
}
