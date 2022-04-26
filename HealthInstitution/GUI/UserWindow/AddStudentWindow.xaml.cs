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
    /// Interaction logic for AddStudentWindow.xaml
    /// </summary>
    public partial class AddStudentWindow : Window
    {
        String usernameInput;
        String passwordInput;
        String nameInput;
        String surnameInput;
        String heightInput;
        String weightInput;
        String illnesesInput;
        String allergensInput;

        public AddStudentWindow()
        {
            InitializeComponent();
        }

        private void saveButton_click(object sender, RoutedEventArgs e)
        {
            usernameInput = usernameBox.Text.Trim();
            passwordInput = passwordBox.Text.Trim();
            nameInput = nameBox.Text.Trim();
            surnameInput = surnameBox.Text.Trim();
            heightInput = heightBox.Text.Trim();
            weightInput = weightBox.Text.Trim();
            illnesesInput = illnessesBox.Text.Trim();
            allergensInput = allergensInput.Trim();

            // TODO: if username already exists

        }
    }
}
