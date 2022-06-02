using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for EditOperationDialog.xaml
    /// </summary>
    public partial class EditOperationDialog : Window
    {
        private Operation _selectedOperation;

        public EditOperationDialog(Operation operation)
        {
            this._selectedOperation = operation;
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            datePicker.SelectedDate = _selectedOperation.Appointment;
            durationTextBox.Text = _selectedOperation.Duration.ToString();
        }

        private void PatientComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var patientComboBox = sender as System.Windows.Controls.ComboBox;
            List<Patient> patients = PatientRepository.GetInstance().Patients;
            foreach (Patient patient in patients)
            {
                patientComboBox.Items.Add(patient);
            }
            patientComboBox.SelectedItem = _selectedOperation.MedicalRecord.Patient;
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
            hourComboBox.SelectedItem = _selectedOperation.Appointment.Hour.ToString();
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
            String operationMinutes = this._selectedOperation.Appointment.Minute.ToString();
            if (operationMinutes.Length == 1)
            {
                operationMinutes = operationMinutes + "0";
            }
            minuteComboBox.SelectedItem = operationMinutes;
        }

        private OperationDTO CreateOperationDTOFromInputData()
        {
            DateTime appointment = (DateTime)datePicker.SelectedDate;
            int minutes = Int32.Parse(minuteComboBox.Text);
            int hours = Int32.Parse(hourComboBox.Text);
            appointment = appointment.AddHours(hours);
            appointment = appointment.AddMinutes(minutes);
            int duration = Int32.Parse(durationTextBox.Text);
            Patient patient = (Patient)patientComboBox.SelectedItem;
            MedicalRecord medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
            return new OperationDTO(appointment, duration, null, _selectedOperation.Doctor, medicalRecord);
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OperationDTO operationDTO = CreateOperationDTOFromInputData();
                operationDTO.Validate();
                OperationRepository.GetInstance().Update(this._selectedOperation.Id, operationDTO);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
