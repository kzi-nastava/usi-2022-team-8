using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
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
            Load();
        }

        public void Load()
        {
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

        private ExaminationDTO CreateExaminationDTOFromInputData()
        {
            DateTime appointment = (DateTime)datePicker.SelectedDate;
            int minutes = Int32.Parse(minuteComboBox.Text);
            int hours = Int32.Parse(hourComboBox.Text);
            appointment = appointment.AddHours(hours);
            appointment = appointment.AddMinutes(minutes);
            Patient patient = (Patient)patientComboBox.SelectedItem;
            MedicalRecord medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
            var doctor = _selectedExamination.Doctor;
            ExaminationDTO examination = new ExaminationDTO(appointment, null, doctor, medicalRecord);
            return examination;
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
                {
                ExaminationDTO examinationDTO = CreateExaminationDTOFromInputData();
                examinationDTO.Validate();
                ExaminationService.Update(_selectedExamination.Id, examinationDTO);
                this.Close();
            }
            catch (Exception ex)
            {
              System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
