using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.Core.TrollCounters.Repository;
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

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for CreatePatientDialog.xaml
    /// </summary>
    public partial class CreatePatientDialog : Window
    {
        public CreatePatientDialog()
        {
            InitializeComponent();
        }

        private UserDTO CreateUserDTOFromInputData()
        {
            UserRepository userRepository = UserRepository.GetInstance();
            string username = usernameBox.Text.Trim();
            string password = passwordBox.Password.ToString().Trim();
            string name = nameBox.Text.Trim();
            string surname = surnameBox.Text.Trim();
            if (username == "" || password == "" || name == "" || surname == "")
            {
                System.Windows.MessageBox.Show("All fields excluding Allergens and Previous ilnesses must be filled!", "Create patient error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception();
            }
            else if (userRepository.UsersByUsername.ContainsKey(username))
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
            string height = heightBox.Text.Trim();
            string weight = weightBox.Text.Trim();
            if (height == "" || weight == "")
            {
                System.Windows.MessageBox.Show("All fields excluding Allergens and Previous ilnesses must be filled!", "Create patient error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception();
            }
            try
            {
                double heightValue = Convert.ToDouble(height);
                double weightValue = Convert.ToDouble(weight);
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
                MedicalRecordDTO medicalRecordDTO = new MedicalRecordDTO(heightValue, weightValue, allergens, previousIlnesses, null);
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
                UserRepository userRepository = UserRepository.GetInstance();
                PatientRepository patientRepository = PatientRepository.GetInstance();
                UserDTO userDTO = CreateUserDTOFromInputData();
                MedicalRecordDTO medicalRecordDTO = CreateMedicalRecordDTOFromInputData();
                PatientService.Add(userDTO, medicalRecordDTO);
                userRepository.Add(userDTO);
                TrollCounterFileRepository.GetInstance().Add(userDTO.Username);
                this.Close();
            }
            catch
            {

            }
        }
    }
}
