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

            var patientWindow = DIContainer.GetService<PatientWindow>();
            patientWindow.SetLoggedPatient(loggedPatient);
            patientWindow.ShowDialog();
            
        }

        private void RedirectDoctor()
        {
            _doctorService.LoadAppointments();
            _doctorService.LoadNotifications();
            Doctor loggedDoctor = _doctorService.GetById(_usernameInput);

            var doctorWindow = DIContainer.GetService<DoctorWindow>();
            doctorWindow.SetLoggedDoctor(loggedDoctor);
            doctorWindow.ShowDialog();
        }

        private void RedirectSecretary()
        {
            _restRequestService.LoadRequests();

            var secretaryWindow = DIContainer.GetService<SecretaryWindow>();
            secretaryWindow.ShowDialog();
        }

        private void RedirectManager()
        {
            var managerWindow = DIContainer.GetService<ManagerWindow>();
            managerWindow.ShowDialog();
        }

        public async Task StartAsync()
        {
            DIContainer.GetService<IEquipmentTransferRefreshingService>().UpdateByTransfer();
            DIContainer.GetService<IRenovationRefreshingService>().UpdateByRenovation();

            //_equipmentTransferRefreshingService.UpdateByTransfer();
            //_renovationRefreshingService.UpdateByRenovation();

            var loginWindow = DIContainer.GetService<LoginWindow>();
            loginWindow.ShowDialog();
        }
    }
}