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
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Notifications.Model;

using HealthInstitution.Core.SystemUsers.Patients.Model;

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        private Patient _loggedPatient;

        public PatientWindow(Patient loggedPatient)
        {
            InitializeComponent();
            this._loggedPatient = loggedPatient;
            ShowNotificationsDialog();
            new RecepieNotificationDialog(loggedPatient.Username).ShowDialog();
        }

        private void ShowNotificationsDialog()
        {
            //ExaminationDoctorRepository.GetInstance();
            int activeNotifications = 0;
            foreach (AppointmentNotification notification in this._loggedPatient.Notifications)
            {
                if (notification.ActiveForPatient)
                    activeNotifications++;
            }
            if (activeNotifications > 0)
            {
                PatientNotificationsDialog patientNotificationsDialog = new PatientNotificationsDialog(this._loggedPatient);
                patientNotificationsDialog.ShowDialog();
            }
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

        private void manuallSchedule_Click(object sender, RoutedEventArgs e)
        {
            new PatientScheduleWindow(this._loggedPatient).ShowDialog();
        }

        private void recommendedSchedule_Click(object sender, RoutedEventArgs e)
        {
            new RecommendedWindow(this._loggedPatient).ShowDialog();
        }

        private void medicalRecordView_button_Click(object sender, RoutedEventArgs e)
        {
            new MedicalRecordView(_loggedPatient).ShowDialog();
        }

        private void pickDoctor_button_Click(object sender, RoutedEventArgs e)
        {
            new DoctorPickExamination(_loggedPatient).ShowDialog();
        }

        private void recepieNotificationSettings_button_Click(object sender, RoutedEventArgs e)
        {
            new RecepieNotificationSettingsDialog(_loggedPatient.Username).ShowDialog();
        }
    }
}