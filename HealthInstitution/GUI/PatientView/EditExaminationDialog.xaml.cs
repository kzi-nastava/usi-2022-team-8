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
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.Examinations.Repository;

namespace HealthInstitution.GUI.PatientWindows
{
    /// <summary>
    /// Interaction logic for EditExaminationDialog.xaml
    /// </summary>
    ///

    public partial class EditExaminationDialog : Window
    {
        private string minutes;
        private string hours;
        private User loggedPatient;
        private string doctorUsername;
        public Examination examination { get; set; }

        public EditExaminationDialog(Examination selectedExamination)
        {
            examination = selectedExamination;
            loggedPatient = selectedExamination.medicalRecord.patient;
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string formatDate = datePicker.SelectedDate.ToString();
            formatDate = formatDate + "T" + hours + ":" + minutes;

            DateTime.TryParse(formatDate, out var dateTime);
            try
            {
                ExaminationRepository.GetInstance().ReserveExamiantion(loggedPatient.username, doctorUsername, dateTime);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Question", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DoctorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var doctorComboBox = sender as System.Windows.Controls.ComboBox;
            List<User> doctors = new List<User>();

            foreach (User user in UserRepository.GetInstance().GetUsers())
            {
                if (user.type == UserType.Doctor)
                    doctors.Add(user);
            }

            doctorComboBox.ItemsSource = doctors;
            doctorComboBox.SelectedItem = examination.doctor;
            doctorComboBox.Items.Refresh();
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
            hourComboBox.SelectedValue = examination.appointment.Hour;
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

            if (examination.appointment.Minute == 0) minuteComboBox.SelectedIndex = 0;
            if (examination.appointment.Minute == 15) minuteComboBox.SelectedIndex = 1;
            if (examination.appointment.Minute == 30) minuteComboBox.SelectedIndex = 2;
            if (examination.appointment.Minute == 45) minuteComboBox.SelectedIndex = 3;
        }

        private void DoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var doctorComboBox = sender as System.Windows.Controls.ComboBox;
            this.doctorUsername = (doctorComboBox.SelectedValue as Doctor).username;
        }

        private void HourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var hourComboBox = sender as System.Windows.Controls.ComboBox;
            int h = hourComboBox.SelectedIndex;
            if (h == 9) hours = "09";
            else
                hours = h.ToString();
        }

        private void MinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            minutes = minuteComboBox.SelectedValue.ToString();
        }
    }
}