using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.Polls;
using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
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

namespace HealthInstitution.GUI.ManagerView.PollView
{
    /// <summary>
    /// Interaction logic for DoctorPollWindow.xaml
    /// </summary>
    public partial class DoctorPollWindow : Window
    {
        public DoctorPollWindow()
        {
            InitializeComponent();
        }

        private void ShowPoll_Click(object sender, RoutedEventArgs e)
        {
            if (doctorComboBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Doctor was not selected!", "Poll error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {    
            Doctor doctor = (Doctor)doctorComboBox.SelectedItem;

            List<TableItemPoll> questions = PollService.GetDoctorPollByQuestions(doctor);
            pollDataGrid.ItemsSource = questions;

            List<PollComment> comments = PollService.GetHospitalComments();
            commentDataGrid.ItemsSource = comments;
        }

        private void TopRated_Click(object sender, RoutedEventArgs e)
        {
            var topRatedDoctors = DoctorRatingsService.GetTopRated(3);
            RatedDoctorsWindow ratedDoctorsWindow = new RatedDoctorsWindow(topRatedDoctors);
            ratedDoctorsWindow.ShowDialog();
        }

        private void WorstRated_Click(object sender, RoutedEventArgs e)
        {
            var worstRatedDoctors = DoctorRatingsService.GetWorstRated(3);
            RatedDoctorsWindow ratedDoctorsWindow = new RatedDoctorsWindow(worstRatedDoctors);
            ratedDoctorsWindow.ShowDialog();
        }

        private void DoctorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<Doctor> doctors = DoctorService.GetAll();
            doctorComboBox.ItemsSource = doctors;
            doctorComboBox.SelectedItem = null;
        }
    }
}
