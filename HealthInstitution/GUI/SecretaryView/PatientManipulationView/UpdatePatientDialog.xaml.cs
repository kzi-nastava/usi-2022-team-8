﻿using HealthInstitution.Core.SystemUsers.Patients;
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
        IPatientService _patientService;
        Patient _selectedPatient;
        public UpdatePatientWindow(IPatientService patientService)
        {
            InitializeComponent();
            _patientService = patientService;
            
        }
        private void LoadInputBoxes()
        {
            usernameBox.Text = _selectedPatient.Username;
            passwordBox.Password = _selectedPatient.Password;
            nameBox.Text = _selectedPatient.Name;
            surnameBox.Text = _selectedPatient.Surname;
        }
        public void SetSelectedPatient(Patient patient)
        {
            _selectedPatient= patient;
            LoadInputBoxes();
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
                UserDTO userDTO = CreateUserDTOFromInputData();
                _patientService.Update(userDTO);
                Close();
            }
            catch
            {

            }
        }
    }
}
