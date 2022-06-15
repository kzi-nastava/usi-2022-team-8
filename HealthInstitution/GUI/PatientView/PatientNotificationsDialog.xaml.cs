using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
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

namespace HealthInstitution.GUI.PatientView
{
    /// <summary>
    /// Interaction logic for PatientNotificationsDialog.xaml
    /// </summary>
    public partial class PatientNotificationsDialog : Window
    {
        private Patient _loggedPatient;
        IAppointmentNotificationService _appointmentNotificationService;
        IPatientService _patientService;
        public PatientNotificationsDialog(IPatientService patientService, IAppointmentNotificationService appointmentNotificationService)
        {
            _patientService = patientService;   
            _appointmentNotificationService = appointmentNotificationService;
            InitializeComponent();
            LoadRows();
        }
        public void SetLoggedPatient(Patient patient)
        {
            _loggedPatient = patient;
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            foreach (AppointmentNotification notification in _patientService.GetActiveAppointmentNotification(_loggedPatient))
            {
                dataGrid.Items.Add(notification);
                _appointmentNotificationService.ChangeActiveStatus(notification, false);
            }
            dataGrid.Items.Refresh();
            _patientService.DeleteNotifications(_loggedPatient);
        }
    }
}
