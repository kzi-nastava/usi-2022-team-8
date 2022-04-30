using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.GUI.PatientWindows;
using System.Windows;

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        private User loggedPatient;

        public PatientWindow(User loggedPatient)
        {
            InitializeComponent();
            ExaminationDoctorRepository.GetInstance();
            this.loggedPatient = loggedPatient;
        }

        private void logout_button_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                this.Close();
                new LoginWindow.LoginWindow().ShowDialog();
            }
        }

        private void Schedule_Button_Click(object sender, RoutedEventArgs e)
        {
            /* var check = TrollCounterRepository.GetInstance().GetTrollCounterById(loggedPatient.username);
             Console.WriteLine(check.ToString());*/

            new PatientScheduleWindow(this.loggedPatient).ShowDialog();
        }
    }
}