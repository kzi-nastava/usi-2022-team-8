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
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.RecommededDTO;

namespace HealthInstitution.GUI.PatientView
{
    /// <summary>
    /// Interaction logic for RecommendedWindow.xaml
    /// </summary>
    ///
    public partial class RecommendedWindow : Window
    {
        private int _startMinutes;
        private int _startHours;
        private int _endMinutes;
        private int _endHours;
        private User _loggedPatient;
        private string _doctorUsername;

        public RecommendedWindow(User loggedPatient)
        {
            InitializeComponent();
            _loggedPatient = loggedPatient;
        }

        private void DoctorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var doctorComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> doctors = new List<String>();

            foreach (Doctor doctor in DoctorRepository.GetInstance().GetAll())
                doctors.Add(doctor.Username);

            doctorComboBox.ItemsSource = doctors;
            doctorComboBox.SelectedIndex = 0;
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
            hourComboBox.SelectedIndex = 0;
            hourComboBox.Items.Refresh();
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
            minuteComboBox.Items.Refresh();
        }

        private void EndMinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            this._endMinutes = minuteComboBox.SelectedIndex * 15;
        }

        private void StartMinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            this._startMinutes = minuteComboBox.SelectedIndex * 15;
        }

        private void DoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var doctorComboBox = sender as System.Windows.Controls.ComboBox;
            _doctorUsername = doctorComboBox.SelectedValue as string;
        }

        private void EndHourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var hourComboBox = sender as System.Windows.Controls.ComboBox;
            this._endHours = hourComboBox.SelectedIndex + 9;
        }

        private void StartHourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var hourComboBox = sender as System.Windows.Controls.ComboBox;
            this._startHours = hourComboBox.SelectedIndex + 9;
        }

        private FirstFitDTO GenerateFirstFitDTO(DateTime dateTime)
        {
            return new FirstFitDTO(_startHours, _startMinutes, dateTime, _endHours, _endMinutes, 23, _loggedPatient.Username, _doctorUsername);
        }

        private ClosestFitDTO GenerateClosestFitDTO(DateTime dateTime, bool doctorPriority)
        {
            return new ClosestFitDTO(_startHours, _startMinutes, dateTime, _endHours, _endMinutes, 23, _loggedPatient.Username, _doctorUsername, doctorPriority);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string formatDate = datePicker.SelectedDate.ToString();
            DateTime.TryParse(formatDate, out var dateTime);
            var fitDTO = GenerateFirstFitDTO(dateTime);
            bool found = ExaminationRepository.GetInstance().FindFirstFit(fitDTO);
            if (!found)
            {
                bool doctorPriority = doctorRadioButton.IsChecked == true;
                var closestFitDTO = GenerateClosestFitDTO(dateTime, doctorPriority);
                List<Examination> suggestions =
                    ExaminationRepository.GetInstance().FindClosestFit(closestFitDTO);
                new ClosestFit(suggestions).ShowDialog();
            }
            this.Close();
        }
    }
}