using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
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
        public EditExaminationDialog(Examination selectedExamination)
        {
            this.selectedExamination = selectedExamination;
            InitializeComponent();
        }

        private void PatientComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var patientComboBox = sender as System.Windows.Controls.ComboBox;
            List<Patient> patients = PatientRepository.GetInstance().patients;
            foreach (Patient patient in patients)
            {
                patientComboBox.Items.Add(patient);
            }
            patientComboBox.SelectedIndex = 0;
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = (DateTime)datePicker.SelectedDate;
            int minutes = Int32.Parse(minuteComboBox.Text);
            int hours = Int32.Parse(hourComboBox.Text);
            dateTime = dateTime.AddHours(hours);
            dateTime = dateTime.AddMinutes(minutes);

            Patient patient = (Patient)patientComboBox.SelectedItem;
            try
            {
                ExaminationRepository.GetInstance().EditExamination(selectedExamination, patient.username, selectedExamination.doctor.username, dateTime);
                //ExaminationDoctorRepository.GetInstance().SaveExaminationDoctor();
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
