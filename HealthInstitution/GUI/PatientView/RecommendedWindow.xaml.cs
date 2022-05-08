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

namespace HealthInstitution.GUI.PatientView
{
    /// <summary>
    /// Interaction logic for RecommendedWindow.xaml
    /// </summary>
    ///
    public partial class RecommendedWindow : Window
    {
        private int _minutes;
        private int _hours;
        private User _loggedPatient;
        private string _doctorUsername;

        public RecommendedWindow()
        {
            InitializeComponent();
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

        private void HourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var hourComboBox = sender as System.Windows.Controls.ComboBox;
            int h = hourComboBox.SelectedIndex;
            this._hours = h + 9;
        }

        private void EndMinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            int m = minuteComboBox.SelectedIndex;
            this._minutes = m * 15;
        }

        private void StartMinuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var minuteComboBox = sender as System.Windows.Controls.ComboBox;
            int m = minuteComboBox.SelectedIndex;
            this._minutes = m * 15;
        }

        private void DoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void EndHourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void StartHourComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}