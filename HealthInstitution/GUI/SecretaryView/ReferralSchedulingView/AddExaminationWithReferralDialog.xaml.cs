using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.Scheduling;
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
            List<string> hours = new List<String>();
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
        private DateTime? GetAppointmentFromInputData()
        {
            DateTime appointment = (DateTime)datePicker.SelectedDate;
            int minutes = int.Parse(minuteComboBox.Text);
            int hours = int.Parse(hourComboBox.Text);
            appointment = appointment.AddHours(hours);
            appointment = appointment.AddMinutes(minutes);
            if (appointment <= DateTime.Now)
            {
                System.Windows.MessageBox.Show("You have to change dates for upcoming ones!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
            else
                return appointment;
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            DateTime? appointmentFromForm=GetAppointmentFromInputData();
            if(appointmentFromForm!=null)
            {
                try
                {
                    DateTime appointment = (DateTime)appointmentFromForm;
                    SchedulingService.RedirectByType(_referral, appointment, _medicalRecord);
                    Close();
                }
                catch(Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            
        }
    }
}
