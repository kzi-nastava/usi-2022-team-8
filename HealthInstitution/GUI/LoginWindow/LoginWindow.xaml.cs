using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.GUI.UserWindow;
using HealthInstitution.GUI.DoctorView;
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
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.ScheduleEditRequests.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.GUI.LoginWindow
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>

    public partial class LoginWindow : Window
    {
        private String usernameInput;
        private String passwordInput;
        public UserRepository userRepository = UserRepository.GetInstance();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginButton_click(object sender, RoutedEventArgs e)
        {
            usernameInput = usernameBox.Text;
            passwordInput = passwordBox.Password.ToString();
            User foundUser = userRepository.GetUserByUsername(usernameInput);
            if (foundUser == null)
            {
                System.Windows.MessageBox.Show("Username doesn't exist!", "Log in error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (foundUser.password != passwordInput)
            {
                System.Windows.MessageBox.Show("Username and password don't match!", "Log in error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                switch (foundUser.type)
                {
                    case UserType.Patient:
                        /*PatientRepository patientRepository = PatientRepository.GetInstance();
                        Patient patient = patientRepository.GetPatientById(usernameInput);*/
                        this.Close();

                        new PatientWindow(foundUser).ShowDialog();
                        break;

                    case UserType.Doctor:
                        this.Close();
                        DoctorWindow doctorWindow = new DoctorWindow();
                        doctorWindow.ShowDialog();
                        DoctorRepository doctorRepository = DoctorRepository.GetInstance();
                        Doctor loggedDoctor = doctorRepository.GetDoctorByUsername(usernameInput);
                        DoctorWindow window = new DoctorWindow(loggedDoctor);
                        window.ShowDialog();

                        break;

                    case UserType.Secretary:
                        this.Close();
                        SecretaryWindow secretaryWindow = new SecretaryWindow();
                        secretaryWindow.ShowDialog();
                        break;

                    case UserType.Manager:
                        this.Close();
                        ManagerWindow managerWindow = new ManagerWindow();
                        managerWindow.ShowDialog();
                        break;
                }
            }
        }

        [STAThread]
        private static void Main(string[] args)
        {
            LoginWindow window = new LoginWindow();
            window.ShowDialog();
        }*/
    }
}