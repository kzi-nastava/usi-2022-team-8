using HealthInstitution.Core.Examinations.Repository;
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
        private User _loggedPatient;
        private string _doctorUsername;

        public AddExaminationDialog(User loggedPatient)
        {
            InitializeComponent();
            this._loggedPatient = loggedPatient;
        }

        private void hourComboBox_Loaded(object sender, RoutedEventArgs e)
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

        private void hourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var hourComboBox = sender as System.Windows.Controls.ComboBox;
            int h = hourComboBox.SelectedIndex;
            _hours = h + 9;
        }

        private void minuteComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> minutes = new List<String>();
            minutes.Add("00");
            minutes.Add("15");
            minutes.Add("30");
            minutes.Add("45");
            minuteComboBox.ItemsSource = minutes;
        }

        private void doctorComboBox_Loaded(object sender, RoutedEventArgs e)
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

        private void create_Click(object sender, RoutedEventArgs e)
        {
            string formatDate = datePicker.SelectedDate.ToString();
            formatDate = formatDate;

            DateTime.TryParse(formatDate, out var dateTime);
            dateTime = dateTime.AddHours(_hours);
            dateTime = dateTime.AddMinutes(_minutes);
            try
            {
                ExaminationRepository.GetInstance().ReserveExamination(_loggedPatient.Username, _doctorUsername, dateTime);
                ExaminationDoctorRepository.GetInstance().Save();
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void minuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            int m = minuteComboBox.SelectedIndex;
            this._minutes = m * 15;
        }

        private void doctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var doctorComboBox = sender as System.Windows.Controls.ComboBox;
            this._doctorUsername = doctorComboBox.SelectedValue as string;
        }
    }
}