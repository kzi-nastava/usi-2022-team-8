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
using System.Windows.Forms;
using HealthInstitution.GUI.LoginView;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Notifications.Model;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        private Doctor _loggedDoctor;
        public DoctorWindow(Doctor doctor)
        {
            InitializeComponent();
            this._loggedDoctor = doctor;
            ShowNotificationsDialog();
        }
        private void ShowNotificationsDialog()
        {
            int activeNotifications = 0;
            foreach (AppointmentNotification notification in this._loggedDoctor.Notifications)
            {
                if (notification.ActiveForDoctor)
                    activeNotifications++;
            }
            if (activeNotifications > 0)
            {
                DoctorNotificationsDialog doctorNotificationsDialog = new DoctorNotificationsDialog(this._loggedDoctor);
                doctorNotificationsDialog.ShowDialog();
            }
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            var answer = System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                this.Close();
                LoginWindow window = new LoginWindow();
                window.ShowDialog();
            }
        }

        private void Examinations_Click(object sender, RoutedEventArgs e)
        {
            new ExaminationTable(this._loggedDoctor).ShowDialog();
        }

        private void Operations_Click(object sender, RoutedEventArgs e)
        {
            new OperationTable(this._loggedDoctor).ShowDialog();
        }

        private void ScheduleReview_Click(object sender, RoutedEventArgs e)
        {
            new ScheduledExaminationTable(this._loggedDoctor).ShowDialog();
        }

        private void ManageDrugs_Click(object sender, RoutedEventArgs e)
        {
            new DrugsVerificationTable().ShowDialog();
        }
    }

}
