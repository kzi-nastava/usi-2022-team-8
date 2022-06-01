using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using System.Windows;

namespace HealthInstitution.GUI.SecretaryView
{
    /// <summary>
    /// Interaction logic for UpdatePatientWindow.xaml
    /// </summary>
    public partial class UpdatePatientWindow : Window
    {
        public Patient Patient { get; set; }
        public UpdatePatientWindow(Patient patient)
        {
            InitializeComponent();
            this.Patient = patient;
            usernameBox.Text = patient.Username;
            passwordBox.Password = patient.Password;
            nameBox.Text = patient.Name;
            surnameBox.Text = patient.Surname;
        }
        private UserDTO CreateUserDTOFromInputData()
        {
            string username = usernameBox.Text.Trim();
            string password = passwordBox.Password.ToString().Trim();
            string name = nameBox.Text.Trim();
            string surname = surnameBox.Text.Trim();
            if (username == "" || password == "" || name == "" || surname == "")
            {
                System.Windows.MessageBox.Show("All fields must be filled!", "Create patient error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception();
            }
            UserDTO userDTO = new UserDTO(UserType.Patient, username, password, name, surname);
            return userDTO;
        }
        private void UpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                UserRepository userRepository = UserRepository.GetInstance();
                PatientRepository patientRepository = PatientRepository.GetInstance();
                UserDTO userDTO = CreateUserDTOFromInputData();
                patientRepository.Update(userDTO);
                userRepository.Update(userDTO);
                Close();
            }
            catch
            {

            }
        }
    }
}
