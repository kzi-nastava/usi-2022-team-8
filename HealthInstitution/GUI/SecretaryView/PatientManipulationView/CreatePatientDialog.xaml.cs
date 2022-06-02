using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.Core.TrollCounters.Repository;
using System.Windows;

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for CreatePatientDialog.xaml
    /// </summary>
    public partial class CreatePatientDialog : Window
    {
        PatientRepository _patientRepository = PatientRepository.GetInstance();
        UserRepository _userRepository=UserRepository.GetInstance();   
        public CreatePatientDialog()
        {
            InitializeComponent();
        }

        private UserDTO CreateUserDTOFromInputData()
        {
            string username = usernameBox.Text.Trim();
            string password = passwordBox.Password.ToString().Trim();
            string name = nameBox.Text.Trim();
            string surname = surnameBox.Text.Trim();
            if (username == "" || password == "" || name == "" || surname == "")
            {
                System.Windows.MessageBox.Show("All fields excluding Allergens and Previous ilnesses must be filled!", "Create patient error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception();
            }
            else if (_userRepository.UsersByUsername.ContainsKey(username))
            {
                System.Windows.MessageBox.Show("This username is already used!", "Create patient error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception();
            }
            UserDTO userDTO = new UserDTO(UserType.Patient, username, password, name, surname);
            return userDTO;
        }
        private MedicalRecordDTO CreateMedicalRecordDTOFromInputData()
        {
            string allergensNotParsed = allergensBox.Text.Trim();
            string previousIlnessesNotParsed = previousIlnessesBox.Text.Trim();
            try
            {
                double height = Convert.ToDouble(heightBox.Text.Trim());
                double weight = Convert.ToDouble(weightBox.Text.Trim());
                List<string> allergens = new List<string>();
                List<string> previousIlnesses = new List<string>();
                if (allergensNotParsed != "")
                {
                    allergens = allergensNotParsed.Split(",").ToList();
                }
                if (previousIlnessesNotParsed != "")
                {
                    previousIlnesses = previousIlnessesNotParsed.Split(",").ToList();
                }
                MedicalRecordDTO medicalRecordDTO = new MedicalRecordDTO(height, weight, allergens, previousIlnesses, null);
                return medicalRecordDTO;
            }
            catch
            {
                System.Windows.MessageBox.Show("Height and weight must be numbers!", "Create patient error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception();
            }
        }
        private void CreatePatient_Click(object sender, RoutedEventArgs e)
        {
            try {
                UserDTO userDTO = CreateUserDTOFromInputData();
                MedicalRecordDTO medicalRecordDTO = CreateMedicalRecordDTOFromInputData();
                PatientService.Add(userDTO, medicalRecordDTO);
                UserService.Add(userDTO);
                TrollCounterService.Add(userDTO.Username);
                this.Close();
            }
            catch
            {

            }
        }
    }
}
