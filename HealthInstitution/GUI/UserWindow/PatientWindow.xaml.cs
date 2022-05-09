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
using HealthInstitution.GUI.PatientView;

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

        private void logOut_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                this.Close();
                LoginWindow window = new LoginWindow();
                window.ShowDialog();
            }
        }

        private void manuallSchedule_Click(object sender, RoutedEventArgs e)
        {
            new PatientScheduleWindow(this._loggedPatient).ShowDialog();
        }

        private void recommendedSchedule_Click(object sender, RoutedEventArgs e)
        {
            new RecommendedWindow(this._loggedPatient).ShowDialog();
        }
    }
}