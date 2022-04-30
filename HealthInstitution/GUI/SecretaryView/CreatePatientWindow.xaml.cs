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

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for CreatePatientWindow.xaml
    /// </summary>
    public partial class CreatePatientWindow : Window
    {
        public CreatePatientWindow()
        {
            InitializeComponent();
        }

        private void createPatient_click(object sender, RoutedEventArgs e)
        {
            string username = usernameBox.Text.Trim();
            string password=passwordBox.Password.ToString().Trim();
            string name = nameBox.Text.Trim();
            string surname=surnameBox.Text.Trim();
            string allergensNotParsed=allergensBox.Text.Trim();
            string previousIlnessesNotParsed=previousIlnessesBox.Text.Trim();
            try {
                
                if (username == "" || password == "" || name == "" || surname == "")
                {
                    System.Windows.MessageBox.Show("All fields must be filled!", "Create patient error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    double height = Convert.ToDouble(heightBox.Text);
                    double weight = Convert.ToDouble(weightBox.Text);
                    List<string> allergens = new List<string>();
                    List<string> previousIlnesses = new List<string>();
                    if(allergensNotParsed!="")
                    {
                        allergens = allergensNotParsed.Split(",").ToList();
                    }
                    if (previousIlnessesNotParsed != "")
                    {
                        previousIlnesses = previousIlnessesNotParsed.Split(",").ToList();
                    }
                    UserRepository userRepository = UserRepository.GetInstance();
                    PatientRepository patientRepository = PatientRepository.GetInstance();
                    patientRepository.AddPatient(username, password, name, surname, height, weight, allergens, previousIlnesses);
                    userRepository.AddUser(UserType.Patient, username, password, name, surname);
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Height and weight must be numbers!", "Create patient error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            


        }
    }
}
