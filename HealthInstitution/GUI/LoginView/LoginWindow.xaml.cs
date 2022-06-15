using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.GUI.UserWindow;
using HealthInstitution.GUI.DoctorView;
using System.Windows;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.TrollCounters.Repository;
using HealthInstitution.Core.EquipmentTransfers.Functionality;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.Renovations;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.DIContainer;

namespace HealthInstitution.GUI.LoginView
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>

    public partial class LoginWindow : Window
    {
        private String _usernameInput;
        private String _passwordInput;

        IUserService _userService;
        ITrollCounterService _trollCounterService;
        IPatientService _patientService;
        IDoctorService _doctorService;
        IEquipmentTransferRefreshingService _equipmentTransferRefreshingService;
        IRenovationRefreshingService _renovationRefreshingService;
        IPrescriptionNotificationService _prescriptionNotificationService;
        IRestRequestService _restRequestService;
        IDoctorRatingsService _doctorRatingsService;

        public LoginWindow(IUserService userService, ITrollCounterService trollCounterService, IPatientService patientService, IDoctorService doctorService, IEquipmentTransferRefreshingService equipmentTransferRefreshingService, IRenovationRefreshingService renovationRefreshingService, IPrescriptionNotificationService prescriptionNotificationService, IRestRequestService restRequestService, IDoctorRatingsService doctorRatingsService)
        {
            _userService = userService;
            _trollCounterService = trollCounterService;
            _patientService = patientService;
            _doctorService = doctorService;
            _equipmentTransferRefreshingService = equipmentTransferRefreshingService;
            _renovationRefreshingService = renovationRefreshingService;
            _prescriptionNotificationService = prescriptionNotificationService;
            _restRequestService = restRequestService;
            _doctorRatingsService = doctorRatingsService;
        }

        private User GetUserFromInputData()
        {
            _usernameInput = usernameBox.Text;
            _passwordInput = passwordBox.Password.ToString();
            return _userService.GetByUsername(_usernameInput);
        }

        private void LoginButton_click(object sender, RoutedEventArgs e)
        {
            User user = GetUserFromInputData();
            if (_userService.IsUserFound(user, _passwordInput) && !_userService.IsUserBlocked(user))
            {
                this.Close();
                switch (user.Type)
                {
                    case UserType.Patient:
                        RedirectPatient(user);
                        break;

                    case UserType.Doctor:
                        RedirectDoctor();
                        break;

                    case UserType.Secretary:
                        RedirectSecretary();
                        break;

                    case UserType.Manager:
                        RedirectManager();
                        break;
                }
            }
        }

        private void RedirectPatient(User foundUser)
        {
            _trollCounterService.TrollCheck(foundUser.Username);
            _patientService.LoadNotifications();
            Patient loggedPatient = _patientService.GetByUsername(_usernameInput);
            _prescriptionNotificationService.GenerateAllSkippedNotifications(loggedPatient.Username);
            _doctorRatingsService.AssignScores();
            new PatientWindow(loggedPatient).ShowDialog();
        }

        private void RedirectDoctor()
        {
            _doctorService.LoadAppointments();
            _doctorService.LoadNotifications();
            Doctor loggedDoctor = _doctorService.GetById(_usernameInput);
            new DoctorWindow(loggedDoctor).ShowDialog();
        }

        private void RedirectSecretary()
        {
            _restRequestService.LoadRequests();
            SecretaryWindow secretaryWindow = new SecretaryWindow();
            secretaryWindow.ShowDialog();
        }

        private void RedirectManager()
        {
            ManagerWindow managerWindow = new ManagerWindow();
            managerWindow.ShowDialog();
        }

        public async Task StartAsync()
        {
            _equipmentTransferRefreshingService.UpdateByTransfer();
            _renovationRefreshingService.UpdateByRenovation();

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
}