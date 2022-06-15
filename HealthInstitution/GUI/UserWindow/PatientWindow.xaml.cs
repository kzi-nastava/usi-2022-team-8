using HealthInstitution.GUI.PatientWindows;
using System.Windows;

using HealthInstitution.GUI.LoginView;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Polls;

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        private Patient _loggedPatient;
        IPatientService _patientService;
        public PatientWindow(Patient loggedPatient, IPatientService patientService)
        {
            InitializeComponent();
            this._loggedPatient = loggedPatient;
            this._patientService = patientService;
            ShowNotificationsDialog();
            new RecepieNotificationDialog(loggedPatient.Username, DIContainer.GetService<IPrescriptionNotificationService>()).ShowDialog();
        }

        private void ShowNotificationsDialog()
        {
            if (_patientService.GetActiveAppointmentNotification(_loggedPatient).Count>0)
            {
                PatientNotificationsDialog patientNotificationsDialog = new PatientNotificationsDialog(this._loggedPatient, DIContainer.GetService<IPatientService>(), DIContainer.GetService<IAppointmentNotificationService>());
                patientNotificationsDialog.ShowDialog();
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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

        private void ManuallSchedule_Click(object sender, RoutedEventArgs e)
        {
            new PatientScheduleWindow(this._loggedPatient, DIContainer.GetService<IExaminationService>(), DIContainer.GetService<ITrollCounterService>(), DIContainer.GetService<IScheduleEditRequestsService>()).ShowDialog();
        }

        private void RecommendedSchedule_Click(object sender, RoutedEventArgs e)
        {
            new RecommendedWindow(this._loggedPatient, DIContainer.GetService<IRecommendedSchedulingService>(), DIContainer.GetService<IDoctorService>()).ShowDialog();
        }

        private void MedicalRecordView_button_Click(object sender, RoutedEventArgs e)
        {
            new MedicalRecordView(_loggedPatient, DIContainer.GetService<IExaminationService>()).ShowDialog();
        }

        private void PickDoctor_button_Click(object sender, RoutedEventArgs e)
        {
            new DoctorPickExamination(_loggedPatient, DIContainer.GetService<IDoctorService>()).ShowDialog();
        }

        private void RecepieNotificationSettings_button_Click(object sender, RoutedEventArgs e)
        {
            new RecepieNotificationSettingsDialog(_loggedPatient.Username, DIContainer.GetService<IMedicalRecordService>(), DIContainer.GetService<IPatientService>(), DIContainer.GetService<IPrescriptionNotificationService>()).ShowDialog();
        }

        private void rateHospital_button_Click(object sender, RoutedEventArgs e)
        {
            new PatientHospitalPollDialog(DIContainer.GetService<IPollService>()).ShowDialog();
        }
    }
}