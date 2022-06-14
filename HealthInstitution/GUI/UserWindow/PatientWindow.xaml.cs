using HealthInstitution.GUI.PatientWindows;
using System.Windows;

using HealthInstitution.GUI.LoginView;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Notifications.Model;

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
    }
}