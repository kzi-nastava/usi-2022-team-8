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
        public DoctorWindow(Doctor doctor, IDoctorService doctorService)
        {
            InitializeComponent();
            this._loggedDoctor = doctor;
            this._doctorService = doctorService;
            ShowNotificationsDialog();
        }
        private void ShowNotificationsDialog()
        {
            if (_doctorService.GetActiveAppointmentNotification(_loggedDoctor).Count + _doctorService.GetActiveRestRequestNotification(_loggedDoctor).Count > 0)
            {
                DoctorNotificationsDialog doctorNotificationsDialog = new DoctorNotificationsDialog(this._loggedDoctor, DIContainer.GetService<IDoctorService>(), DIContainer.GetService<IAppointmentNotificationService>(), DIContainer.GetService<IRestRequestNotificationService>());
                doctorNotificationsDialog.ShowDialog();
            }
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            var answer = System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                this.Close();
                LoginWindow window = new LoginWindow(DIContainer.GetService<IUserService>(),
                                                    DIContainer.GetService<ITrollCounterService>(),
                                                    DIContainer.GetService<IPatientService>(),
                                                    DIContainer.GetService<IDoctorService>(),
                                                    DIContainer.GetService<IEquipmentTransferRefreshingService>(),
                                                    DIContainer.GetService<IRenovationRefreshingService>(),
                                                    DIContainer.GetService<IPrescriptionNotificationService>(),
                                                    DIContainer.GetService<IRestRequestService>(),
                                                    DIContainer.GetService<IDoctorRatingsService>());
                window.ShowDialog();
            }
        }

        private void Examinations_Click(object sender, RoutedEventArgs e)
        {
            new ExaminationTable(this._loggedDoctor, DIContainer.GetService<IExaminationService>(), DIContainer.GetService<IDoctorService>()).ShowDialog();
        }

        private void Operations_Click(object sender, RoutedEventArgs e)
        {
            new OperationTable(this._loggedDoctor, DIContainer.GetService<IOperationService>(), DIContainer.GetService<IDoctorService>()).ShowDialog();
        }

        private void ScheduleReview_Click(object sender, RoutedEventArgs e)
        {
            new ScheduledExaminationTable(this._loggedDoctor, DIContainer.GetService<ITimetableService>(), DIContainer.GetService<IExaminationService>()).ShowDialog();
        }
        
        private void ManageDrugs_Click(object sender, RoutedEventArgs e)
        {
            new DrugsVerificationTable(DIContainer.GetService<IDrugVerificationService>(), DIContainer.GetService<IDrugService>()).ShowDialog();
        }
    }

}
