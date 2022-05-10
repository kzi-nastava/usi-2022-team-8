using System.Windows;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for AddOperationDialog.xaml
    /// </summary>
    public partial class AddOperationDialog : Window
    {
        private Doctor _loggedDoctor;
        public AddOperationDialog(Doctor doctor)
        {
            this._loggedDoctor = doctor;
            InitializeComponent();
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
            List<Patient> patients = PatientRepository.GetInstance().Patients;
            foreach (Patient patient in patients)
            {
                patientComboBox.Items.Add(patient);
            }
            patientComboBox.SelectedIndex = 0;
        }

        private void CollectForms()
        {
            DateTime appointment = (DateTime)datePicker.SelectedDate;
            int minutes = Int32.Parse(minuteComboBox.Text);
            int hours = Int32.Parse(hourComboBox.Text);
            appointment = appointment.AddHours(hours);
            appointment = appointment.AddMinutes(minutes);
            int duration = Int32.Parse(durationTextBox.Text);
            Patient patient = (Patient)patientComboBox.SelectedItem;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime appointment = (DateTime)datePicker.SelectedDate;
                int minutes = Int32.Parse(minuteComboBox.Text);
                int hours = Int32.Parse(hourComboBox.Text);
                appointment = appointment.AddHours(hours);
                appointment = appointment.AddMinutes(minutes);
                int duration = Int32.Parse(durationTextBox.Text);
                Patient patient = (Patient)patientComboBox.SelectedItem;
                if (appointment <= DateTime.Now)
                {
                    System.Windows.MessageBox.Show("You have to change dates for upcoming ones!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                } else if (duration <= 15) {
                    System.Windows.MessageBox.Show("Operation can't last less than 15 minutes!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    OperationRepository.GetInstance().ReserveOperation(patient.Username, _loggedDoctor.Username, appointment, duration);
                    OperationDoctorRepository.GetInstance().Save();
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
