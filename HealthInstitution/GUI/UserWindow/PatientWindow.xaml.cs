using HealthInstitution.GUI.PatientWindows;
using System.Windows;

using HealthInstitution.GUI.LoginView;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Polls;

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        private Patient _loggedPatient;
        IPatientService _patientService;
        public PatientWindow(IPatientService patientService)
        {
            InitializeComponent();
            this._loggedPatient = loggedPatient;
            //ShowNotificationsDialog();
        }

        public void SetLoggedPatient(Patient patient)
        {
            _loggedPatient = patient;
            ShowNotificationsDialog();
            RecepieNotificationDialog recepieNotificationDialog = DIContainer.GetService<RecepieNotificationDialog>();
            recepieNotificationDialog.SetLoggedPatient(_loggedPatient.Username);
            recepieNotificationDialog.ShowDialog();
        }
        private void ShowNotificationsDialog()
        {
            if (_patientService.GetActiveAppointmentNotification(_loggedPatient).Count>0)
            {
                PatientNotificationsDialog patientNotificationsDialog = DIContainer.GetService<PatientNotificationsDialog>();
                patientNotificationsDialog.SetLoggedPatient(_loggedPatient);
                patientNotificationsDialog.ShowDialog();
            }
        }
    }
}