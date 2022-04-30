using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
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

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for EditExaminationDialog.xaml
    /// </summary>
    public partial class EditExaminationDialog : Window
    {

        public Examination selectedExamination { get; set; }    
        public EditExaminationDialog(Examination examination)
        {
            this.selectedExamination = examination;
            InitializeComponent();
            datePicker.SelectedDate = this.selectedExamination.appointment.Date;
            datePicker.Text = this.selectedExamination.appointment.Date.ToString();
        }

        private void PatientComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var patientComboBox = sender as System.Windows.Controls.ComboBox;
            List<Patient> patients = PatientRepository.GetInstance().patients;
            foreach (Patient patient in patients)
            {
                patientComboBox.Items.Add(patient);
            }
            patientComboBox.SelectedItem = this.selectedExamination.medicalRecord.patient;
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
            hourComboBox.SelectedItem = this.selectedExamination.appointment.Hour.ToString();
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
            String examinationMinutes = this.selectedExamination.appointment.Minute.ToString();
            if (examinationMinutes.Length == 1)
            {
                examinationMinutes = examinationMinutes + "0";
            }
            minuteComboBox.SelectedItem = examinationMinutes;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime appointment = (DateTime)datePicker.SelectedDate;
                int minutes = Int32.Parse(minuteComboBox.Text);
                int hours = Int32.Parse(hourComboBox.Text);
                appointment = appointment.AddHours(hours);
                appointment = appointment.AddMinutes(minutes);
                Patient patient = (Patient)patientComboBox.SelectedItem;
                MedicalRecord medicalRecord = MedicalRecordRepository.GetInstance().GetMedicalRecordByUsername(patient);
                if (appointment <= DateTime.Now)
                {
                    System.Windows.MessageBox.Show("You have to change dates for upcoming ones!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ExaminationRepository.GetInstance().UpdateExamination(selectedExamination.id, appointment, medicalRecord);
                    //ExaminationDoctorRepository.GetInstance().SaveToFile();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
