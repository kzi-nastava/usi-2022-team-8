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

namespace HealthInstitution.GUI.LoginView
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>

    public partial class LoginWindow : Window
    {
        private String _usernameInput;
        private String _passwordInput;
        private UserRepository _userRepository = UserRepository.GetInstance();

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

        private bool IsUserFound(User user)
        {
            if (user == null)
            {
                System.Windows.MessageBox.Show("Username doesn't exist!", "Log in error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (user.Password != _passwordInput)
            {
                System.Windows.MessageBox.Show("Username and password don't match!", "Log in error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool IsUserBlocked(User user)
        {
            if (user.Blocked != BlockState.NotBlocked)
            {
                System.Windows.MessageBox.Show("Account is blocked!", "Log in error", MessageBoxButton.OK, MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        private void LoginButton_click(object sender, RoutedEventArgs e)
        {
            User user = GetUserFromInputData();
            if (IsUserFound(user) && !IsUserBlocked(user))
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
            AppointmentNotificationDoctorRepository.GetInstance();
            AppointmentNotificationPatientRepository.GetInstance();
            PatientRepository patientRepository = PatientRepository.GetInstance();
            TrollCounterService.TrollCheck(foundUser.Username);
            Patient loggedPatient = patientRepository.GetByUsername(_usernameInput);

            PrescriptionNotificationService.GenerateAllSkippedNotifications(loggedPatient.Username);
            new PatientWindow(loggedPatient).ShowDialog();
        }

        private void RedirectDoctor()
        {
            DoctorRepository doctorRepository = DoctorRepository.GetInstance();
            AppointmentNotificationDoctorRepository.GetInstance();
            AppointmentNotificationPatientRepository.GetInstance();
            OperationDoctorRepository.GetInstance();
            Doctor loggedDoctor = doctorRepository.GetById(_usernameInput);
            new DoctorWindow(loggedDoctor).ShowDialog();
        }

        private void RedirectSecretary()
        {
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