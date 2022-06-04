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
        public PatientNotificationsDialog(Patient patient)
        {
            _loggedPatient = patient;
            InitializeComponent();
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<AppointmentNotification> patientsNotifications = _loggedPatient.Notifications;
            foreach (AppointmentNotification notification in patientsNotifications)
            {
                dataGrid.Items.Add(notification);
                AppointmentNotificationService.ChangeActiveStatus(notification, false);
                PatientService.DeleteNotification(_loggedPatient, notification);
            }
            dataGrid.Items.Refresh();
        }
    }
}
