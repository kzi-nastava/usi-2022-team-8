using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for EditExaminationDialog.xaml
    /// </summary>
    public partial class EditExaminationDialog : Window
    {

        private Examination _selectedExamination; 
        public EditExaminationDialog(Examination examination)
        {
            this._selectedExamination = examination;
            InitializeComponent();
            datePicker.SelectedDate = this._selectedExamination.Appointment.Date;
            datePicker.Text = this._selectedExamination.Appointment.Date.ToString();
        }

        private void PatientComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var patientComboBox = sender as System.Windows.Controls.ComboBox;
            List<Patient> patients = PatientRepository.GetInstance().Patients;
            foreach (Patient patient in patients)
            {
                patientComboBox.Items.Add(patient);
            }
            patientComboBox.SelectedItem = this._selectedExamination.MedicalRecord.Patient;
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
            hourComboBox.SelectedItem = this._selectedExamination.Appointment.Hour.ToString();
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
            String examinationMinutes = this._selectedExamination.Appointment.Minute.ToString();
            if (examinationMinutes.Length == 1)
            {
                examinationMinutes = examinationMinutes + "0";
            }
            minuteComboBox.SelectedItem = examinationMinutes;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            DateTime appointment = (DateTime)datePicker.SelectedDate;
            int minutes = Int32.Parse(minuteComboBox.Text);
            int hours = Int32.Parse(hourComboBox.Text);
            appointment = appointment.AddHours(hours);
            appointment = appointment.AddMinutes(minutes);
            Patient patient = (Patient)patientComboBox.SelectedItem;
            MedicalRecord medicalRecord = MedicalRecordRepository.GetInstance().GetByPatientUsername(patient);
            if (appointment <= DateTime.Now)
            {
                System.Windows.MessageBox.Show("You have to change dates for upcoming ones!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ExaminationRepository.GetInstance().UpdateExamination(_selectedExamination.Id, appointment, medicalRecord);
                //ExaminationDoctorRepository.GetInstance().Save();
                this.Close();
            }
           // }
            //catch (Exception ex)
            //{
              //  System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
        }
    }
}
