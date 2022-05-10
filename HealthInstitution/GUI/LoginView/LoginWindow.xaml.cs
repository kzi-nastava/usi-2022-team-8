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

        private void LoginButton_click(object sender, RoutedEventArgs e)
        {
            _usernameInput = usernameBox.Text;
            _passwordInput = passwordBox.Password.ToString();
            User foundUser = _userRepository.GetByUsername(_usernameInput);
            if (foundUser == null)
            {
                System.Windows.MessageBox.Show("Username doesn't exist!", "Log in error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (foundUser.Password != _passwordInput)
            {
                System.Windows.MessageBox.Show("Username and password don't match!", "Log in error", MessageBoxButton.OK, MessageBoxImage.Error);
            } else if (foundUser.Blocked == BlockState.BlockedBySecretary || foundUser.Blocked == BlockState.BlockedBySystem)
            {
                System.Windows.MessageBox.Show("Account is blocked!", "Log in error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.Close();
                switch (foundUser.Type)
                {
                    case UserType.Patient:
                        try
                        {
                            TrollCounterFileRepository.GetInstance().TrollCheck(foundUser.Username);
                            new PatientWindow(foundUser).ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message, "Troll Alert", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        break;

                    case UserType.Doctor:
                        DoctorRepository doctorRepository = DoctorRepository.GetInstance();
                        ExaminationRepository.GetInstance();
                        ExaminationDoctorRepository.GetInstance();
                        OperationDoctorRepository.GetInstance();
                        Doctor loggedDoctor = doctorRepository.GetById(_usernameInput);
                        new DoctorWindow(loggedDoctor).ShowDialog();

                        break;

                    case UserType.Secretary:
                        DoctorRepository.GetInstance();
                        ExaminationRepository.GetInstance();
                        ExaminationDoctorRepository.GetInstance();
                        OperationDoctorRepository.GetInstance();
                        SecretaryWindow secretaryWindow = new SecretaryWindow();
                        secretaryWindow.ShowDialog();
                        break;

                    case UserType.Manager:
                        ManagerWindow managerWindow = new ManagerWindow();
                        managerWindow.ShowDialog();
                        break;
                }
            }
        }

        [STAThread]
        private static void Main(string[] args)
        {
            EquipmentTransferChecker.UpdateByTransfer();
            LoginWindow window = new LoginWindow();
            window.ShowDialog();
        }
    }
}