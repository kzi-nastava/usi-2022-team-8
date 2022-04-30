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
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;

namespace HealthInstitution.GUI.PatientView
{
    /// <summary>
    /// Interaction logic for AddExaminationDialog.xaml
    /// </summary>
    public partial class AddExaminationDialog : Window
    {
        private int minutes;
        private int hours;
        private User loggedPatient;
        private string doctorUsername;

        public AddExaminationDialog(User loggedPatient)
        {
            InitializeComponent();
            this.loggedPatient = loggedPatient;
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
            hours = h + 9;
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

            foreach (User user in UserRepository.GetInstance().GetUsers())
            {
                if (user.type == UserType.Doctor)
                    doctors.Add(user.username);
            }

            doctorComboBox.ItemsSource = doctors;
            doctorComboBox.SelectedItem = null;
            doctorComboBox.Items.Refresh();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string formatDate = datePicker.SelectedDate.ToString();
            formatDate = formatDate;

            DateTime.TryParse(formatDate, out var dateTime);
            dateTime = dateTime.AddHours(hours);
            dateTime = dateTime.AddMinutes(minutes);
            try
            {
                ExaminationRepository.GetInstance().ReserveExamiantion(loggedPatient.username, doctorUsername, dateTime);
                ExaminationDoctorRepository.GetInstance().SaveExaminationDoctor();
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Question", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            int m = minuteComboBox.SelectedIndex;
            this.minutes = m * 15;
        }

        private void DoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var doctorComboBox = sender as System.Windows.Controls.ComboBox;
            this.doctorUsername = doctorComboBox.SelectedValue as string;
        }
    }
}