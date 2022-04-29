using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.GUI.UserWindow;
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
                        Patient patient = patientRepository.GetPatientById(usernameInput);
                        PatientWindow patientWindow = new PatientWindow();
                        patientWindow.ShowDialog();
                        break;*/
                        Examination temp = ExaminationRepository.GetInstance().GetExaminationById(1);
                        /* Console.WriteLine(temp.GetExaminationById(1));
                         Examination examination = temp.GetExaminationById(1);
                         ExaminationRepository.GetInstance().DeleteExaminations(examination.id);
                         ExaminationRepository.GetInstance().AddExamination(examination.appointment, examination.status, examination.room, examination.doctor, examination.medicalRecord, examination.anamnesis);
                         examination.anamnesis = "AAAAAAAAAAAA";
                         examination.id = 2;
                         ExaminationRepository.GetInstance().SwapExamination(examination);*/

                        ScheduleEditRequestRepository.GetInstance().AddScheduleEditRequests(temp);
                        ScheduleEditRequestRepository.GetInstance().DeleteScheduleEditRequests(1);

                        break;

                    case UserType.Doctor:
                        this.Close();
                        DoctorWindow doctorWindow = new DoctorWindow();
                        doctorWindow.ShowDialog();
                        /*PatientRepository patientRepository = PatientRepository.GetInstance();
                        Patient patient = patientRepository.GetPatientById(usernameInput);
                        PatientWindow patientWindow = new PatientWindow();
                        patientWindow.ShowDialog();
                        break;*/

                        break;

                    case UserType.Secretary:
                        this.Close();
                        SecretaryWindow secretaryWindow = new SecretaryWindow();
                        secretaryWindow.ShowDialog();
                        /*PatientRepository patientRepository = PatientRepository.GetInstance();
                        Patient patient = patientRepository.GetPatientById(usernameInput);
                        PatientWindow patientWindow = new PatientWindow();
                        patientWindow.ShowDialog();
                        break;*/
                        break;

                    case UserType.Manager:
                        this.Close();
                        ManagerWindow managerWindow = new ManagerWindow();
                        managerWindow.ShowDialog();
                        /*PatientRepository patientRepository = PatientRepository.GetInstance();
                        Patient patient = patientRepository.GetPatientById(usernameInput);
                        PatientWindow patientWindow = new PatientWindow();
                        patientWindow.ShowDialog();
                        break;*/
                        break;
                }
            }
        }

        [STAThread]
        private static void Main(string[] args)
        {
            LoginWindow window = new LoginWindow();
            window.ShowDialog();
        }
    }
}