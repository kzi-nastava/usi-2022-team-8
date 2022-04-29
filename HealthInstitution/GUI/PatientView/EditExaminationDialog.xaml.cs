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

namespace HealthInstitution.GUI.PatientWindows
{
    /// <summary>
    /// Interaction logic for EditExaminationDialog.xaml
    /// </summary>
    public partial class EditExaminationDialog : Window
    {
        public Examination examination { get; set; }

        public EditExaminationDialog(Examination selectedExamination)
        {
            examination = selectedExamination;
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            //TODO
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
            hourComboBox.SelectedIndex = 0;
            // TODO stavi sate
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
            // TODO stavi minute
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}