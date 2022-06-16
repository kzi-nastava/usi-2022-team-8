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
using HealthInstitution.Core.RestRequestNotifications.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.RestRequestNotifications;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core;
using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.DoctorRatings;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        private Doctor _loggedDoctor;
        IDoctorService _doctorService;
        public DoctorWindow(IDoctorService doctorService)
        {
            InitializeComponent();
            this._doctorService = doctorService;
        }
        public void SetLoggedDoctor(Doctor doctor)
        {
            _loggedDoctor = doctor;
            ShowNotificationsDialog();
        }
        private void ShowNotificationsDialog()
        {
            if (_doctorService.GetActiveAppointmentNotification(_loggedDoctor).Count + _doctorService.GetActiveRestRequestNotification(_loggedDoctor).Count > 0)
            {
                DoctorNotificationsDialog doctorNotificationsDialog = DIContainer.GetService<DoctorNotificationsDialog>();
                doctorNotificationsDialog.SetLoggedDoctor(this._loggedDoctor);
                doctorNotificationsDialog.ShowDialog();
                    
            }
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            var answer = System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                this.Close();

                LoginWindow window = DIContainer.GetService<LoginWindow>();
                window.ShowDialog();
            }
        }

        private void Examinations_Click(object sender, RoutedEventArgs e)
        {
            ExaminationTable examinationTable = DIContainer.GetService<ExaminationTable>();
            examinationTable.SetLoggedDoctor(_loggedDoctor);
            examinationTable.ShowDialog();           
        }

        private void Operations_Click(object sender, RoutedEventArgs e)
        {
            OperationTable operationTable = DIContainer.GetService<OperationTable>();
            operationTable.SetLoggedDoctor(_loggedDoctor);
            operationTable.ShowDialog();            
        }

        private void ScheduleReview_Click(object sender, RoutedEventArgs e)
        {
            ScheduledExaminationTable scheduledExaminationTable = DIContainer.GetService<ScheduledExaminationTable>();
            scheduledExaminationTable.SetLoggedDoctor(_loggedDoctor);
            scheduledExaminationTable.ShowDialog();          
        }
        
        private void ManageDrugs_Click(object sender, RoutedEventArgs e)
        {
            DrugsVerificationTable drugsVerificationTable = DIContainer.GetService<DrugsVerificationTable>();
            drugsVerificationTable.ShowDialog();
        }

        private void RestRequests_Click(object sender, RoutedEventArgs e)
        {
            RestRequestTable restRequestTable = DIContainer.GetService<RestRequestTable>();
            restRequestTable.SetLoggedDoctor(_loggedDoctor);
            restRequestTable.ShowDialog();
        }
    }

}
