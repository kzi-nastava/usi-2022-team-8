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
        IDoctorRatingsService _doctorRatingsService;
        IPollService _pollService;
        IDoctorService _doctorService;
        public DoctorPollWindow(IDoctorRatingsService doctorRatingsService, IPollService pollService, IDoctorService doctorService)
        {
            InitializeComponent();
            _doctorRatingsService = doctorRatingsService;
            _pollService = pollService;
            _doctorService = doctorService;
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

            List<TableItemPoll> questions = _pollService.GetDoctorPollByQuestions(doctor);
            pollDataGrid.ItemsSource = questions;

            List<PollComment> comments = _pollService.GetCommentsByDoctor(doctor);
            commentDataGrid.ItemsSource = comments;
        }

        private void TopRated_Click(object sender, RoutedEventArgs e)
        {
            var topRatedDoctors = _doctorRatingsService.GetTopRated(3);
            RatedDoctorsWindow ratedDoctorsWindow = new RatedDoctorsWindow(topRatedDoctors);
            ratedDoctorsWindow.ShowDialog();
        }

        private void WorstRated_Click(object sender, RoutedEventArgs e)
        {
            var worstRatedDoctors = _doctorRatingsService.GetWorstRated(3);
            RatedDoctorsWindow ratedDoctorsWindow = new RatedDoctorsWindow(worstRatedDoctors);
            ratedDoctorsWindow.ShowDialog();
        }

        private void DoctorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<Doctor> doctors = _doctorService.GetAll();
            doctorComboBox.ItemsSource = doctors;
            doctorComboBox.SelectedItem = null;
        }
    }
}