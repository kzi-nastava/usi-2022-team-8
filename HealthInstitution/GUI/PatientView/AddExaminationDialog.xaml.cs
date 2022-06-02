using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using System.Windows;
using System.Windows.Controls;

namespace HealthInstitution.GUI.PatientView
{
    /// <summary>
    /// Interaction logic for AddExaminationDialog.xaml
    /// </summary>
    public partial class AddExaminationDialog : Window
    {
        private int _minutes;
        private int _hours;
        private Patient _loggedPatient;
        private string _doctorUsername;

        public AddExaminationDialog(Patient loggedPatient)
        {
            InitializeComponent();
            this._loggedPatient = loggedPatient;
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

        private void HourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var hourComboBox = sender as System.Windows.Controls.ComboBox;
            int h = hourComboBox.SelectedIndex;
            _hours = h + 9;
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
        }

        private void DoctorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var doctorComboBox = sender as System.Windows.Controls.ComboBox;
            List<string> doctors = new List<string>();

            foreach (User user in UserRepository.GetInstance().GetAll())
            {
                if (user.Type == UserType.Doctor)
                    doctors.Add(user.Username);
            }

            doctorComboBox.ItemsSource = doctors;
            doctorComboBox.SelectedItem = null;
            doctorComboBox.Items.Refresh();
        }

        private void CreateExamination(DateTime dateTime)
        {
            MedicalRecord medicalRecord = MedicalRecordService.GetByPatientUsername(_loggedPatient);
            Doctor doctor = DoctorRepository.GetInstance().GetById(_doctorUsername);
            ExaminationDTO examination = new ExaminationDTO(dateTime, null, doctor, medicalRecord);
            ExaminationRepository.GetInstance().ReserveExamination(examination);
            ExaminationDoctorRepository.GetInstance().Save();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string formatDate = datePicker.SelectedDate.ToString();
            formatDate = formatDate;

            DateTime.TryParse(formatDate, out var dateTime);
            dateTime = dateTime.AddHours(_hours);
            dateTime = dateTime.AddMinutes(_minutes);
            try
            {
                CreateExamination(dateTime);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            int m = minuteComboBox.SelectedIndex;
            this._minutes = m * 15;
        }

        private void DoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var doctorComboBox = sender as System.Windows.Controls.ComboBox;
            this._doctorUsername = doctorComboBox.SelectedValue as string;
        }
    }
}