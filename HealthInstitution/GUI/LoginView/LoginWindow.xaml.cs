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
using HealthInstitution.Core.RestRequests;

namespace HealthInstitution.GUI.LoginView
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>

    public partial class LoginWindow : Window
    {
        private String _usernameInput;
        private String _passwordInput;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private User GetUserFromInputData()
        {
            _usernameInput = usernameBox.Text;
            _passwordInput = passwordBox.Password.ToString();
            return UserService.GetByUsername(_usernameInput);
        }

        private void LoginButton_click(object sender, RoutedEventArgs e)
        {
            User user = GetUserFromInputData();
            if (UserService.IsUserFound(user, _passwordInput) && !UserService.IsUserBlocked(user))
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
            TrollCounterService.TrollCheck(foundUser.Username);
            Patient loggedPatient = PatientService.GetByUsername(_usernameInput);
            PatientService.LoadNotifications();
            PrescriptionNotificationService.GenerateAllSkippedNotifications(loggedPatient.Username);
            DoctorRatingsService.AssignScores();
            new PatientWindow(loggedPatient).ShowDialog();
        }

        private void RedirectDoctor()
        {
            DoctorService.LoadAppointments();
            DoctorService.LoadNotifications();
            Doctor loggedDoctor = DoctorService.GetById(_usernameInput);
            new DoctorWindow(loggedDoctor).ShowDialog();
        }

        private void RedirectSecretary()
        {
            RestRequestService.LoadRequests();
            SecretaryWindow secretaryWindow = new SecretaryWindow();
            secretaryWindow.ShowDialog();
        }

        private void RedirectManager()
        {
            ManagerWindow managerWindow = new ManagerWindow();
            managerWindow.ShowDialog();
        }

        [STAThread]
        private static void Main(string[] args)
        {
            EquipmentTransferRefreshingService.UpdateByTransfer();
            RenovationRefreshingService.UpdateByRenovation();

            LoginWindow window = new LoginWindow();
            window.ShowDialog();
        }
    }
}