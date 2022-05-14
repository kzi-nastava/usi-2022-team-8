using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
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

        private void UpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameBox.Text;
            string password = passwordBox.Password.ToString();
            string name = nameBox.Text;
            string surname = surnameBox.Text;
            if (username == "" || password == "" || name == "" || surname == "")
            {
                System.Windows.MessageBox.Show("All fields must be filled!", "Create patient error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                UserRepository userRepository = UserRepository.GetInstance();
                PatientRepository patientRepository = PatientRepository.GetInstance();
                UserDTO userDTO = new UserDTO(UserType.Patient, username, password, name, surname);
                patientRepository.Update(userDTO);
                userRepository.Update(userDTO);
                this.Close();
            }
        }
    }
}
