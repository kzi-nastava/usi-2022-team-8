using System.Windows;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for AddOperationDialog.xaml
    /// </summary>
    public partial class AddOperationDialog : Window
    {
        private Doctor _loggedDoctor;
        IPatientService _patientService;
        IMedicalRecordService _medicalRecordService;
        ISchedulingService _schedulingService;
        public AddOperationDialog(IPatientService patientService,
                                    IMedicalRecordService medicalRecordService,
                                    ISchedulingService schedulingService)
        {
            InitializeComponent();
            _patientService = patientService;
            _medicalRecordService = medicalRecordService;
            _schedulingService = schedulingService;
        }
        public void SetLoggedDoctor(Doctor doctor)
        {

            this._loggedDoctor = doctor;
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

        private void PatientComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            patientComboBox.Items.Clear();
            List<Patient> patients = _patientService.GetAll();
            foreach (Patient patient in patients)
            {
                patientComboBox.Items.Add(patient);
            }
            patientComboBox.SelectedIndex = 0;
        }

        private OperationDTO CreateOperationDTOFromInputData()
        {
            var appointment = (DateTime)datePicker.SelectedDate;
            int minutes = Int32.Parse(minuteComboBox.Text);
            int hours = Int32.Parse(hourComboBox.Text);
            appointment = appointment.AddHours(hours);
            appointment = appointment.AddMinutes(minutes);
            int duration = Int32.Parse(durationTextBox.Text);
            var patient = (Patient)patientComboBox.SelectedItem;
            var medicalRecord = _medicalRecordService.GetByPatientUsername(patient);
            return new OperationDTO(appointment, duration, null, _loggedDoctor, medicalRecord);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OperationDTO operationDTO = CreateOperationDTOFromInputData();
                _schedulingService.ReserveOperation(operationDTO);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
