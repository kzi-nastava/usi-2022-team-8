using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.GUI.PatientWindows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HealthInstitution.GUI.PatientWindows;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.TrollCounters.Repository;
using HealthInstitution.Core.TrollCounters.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.GUI.LoginView;

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        private User _loggedPatient;

        public PatientWindow(User loggedPatient)
        {
            InitializeComponent();
            ExaminationDoctorRepository.GetInstance();
            this._loggedPatient = loggedPatient;
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                this.Close();
                LoginWindow window = new LoginWindow();
                window.ShowDialog();
            }
        }

        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            /* var check = TrollCounterRepository.GetInstance().GetTrollCounterById(loggedPatient.Username);
             Console.WriteLine(check.ToString());*/

            new PatientScheduleWindow(this._loggedPatient).ShowDialog();
        }
    }
}