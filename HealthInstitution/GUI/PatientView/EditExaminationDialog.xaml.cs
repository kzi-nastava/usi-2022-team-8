using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using System.Windows;
using System.Windows.Controls;

namespace HealthInstitution.GUI.PatientWindows
{
    /// <summary>
    /// Interaction logic for EditExaminationDialog.xaml
    /// </summary>
    ///

    public partial class EditExaminationDialog : Window
    {
        private int minutes;
        private int hours;
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
            formatDate = formatDate;

            DateTime.TryParse(formatDate, out var dateTime);
            dateTime = dateTime.AddHours(hours);
            dateTime = dateTime.AddMinutes(minutes);
            try
            {
                if (examination.appointment.AddDays(-2) < DateTime.Today)
                {
                    Examination newExamination = ExaminationRepository.GetInstance().GenerateRequestExamination(examination, loggedPatient.username, doctorUsername, dateTime);
                    ScheduleEditRequestFileRepository.GetInstance().AddEditRequests(newExamination);
                }
                else
                {
                    ExaminationRepository.GetInstance().EditExamination(examination, loggedPatient.username, doctorUsername, dateTime);
                    ExaminationDoctorRepository.GetInstance().SaveToFile();
                    ExaminationRepository.GetInstance().SaveToFile();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Question", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DoctorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var doctorComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> doctors = new List<String>();

            foreach (User user in UserRepository.GetInstance().GetUsers())
            {
                if (user.type == UserType.Doctor)
                    doctors.Add(user.username);
            }

            doctorComboBox.ItemsSource = doctors;
            doctorComboBox.SelectedItem = examination.doctor.username;
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
            hourComboBox.SelectedIndex = examination.appointment.Hour - 9;
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
            minuteComboBox.SelectedIndex = examination.appointment.Minute / 15;
        }

        private void DoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var doctorComboBox = sender as System.Windows.Controls.ComboBox;
            this.doctorUsername = doctorComboBox.SelectedValue as string;
        }

        private void HourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var hourComboBox = sender as System.Windows.Controls.ComboBox;
            int h = hourComboBox.SelectedIndex;
            hours = h + 9;
        }

        private void MinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            int m = minuteComboBox.SelectedIndex;
            this.minutes = m * 15;
        }
    }
}