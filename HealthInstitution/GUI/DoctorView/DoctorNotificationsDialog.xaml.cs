using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.RestRequestNotifications;
using HealthInstitution.Core.RestRequestNotifications.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
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

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for DoctorNotificationsDialog.xaml
    /// </summary>
    public partial class DoctorNotificationsDialog : Window
    {
        private Doctor _loggedDoctor;
        public DoctorNotificationsDialog(Doctor doctor)
        {
            _loggedDoctor = doctor;
            InitializeComponent();
            LoadRows();
        }
        private void LoadRows()
        {
            appointmentGrid.Items.Clear();
            restRequestGrid.Items.Clear();
            foreach (AppointmentNotification notification in DoctorService.GetActiveAppointmentNotification(_loggedDoctor))
            {
                appointmentGrid.Items.Add(notification);
                AppointmentNotificationService.ChangeActiveStatus(notification,true);
            }
            foreach (RestRequestNotification notification in DoctorService.GetActiveRestRequestNotification(_loggedDoctor))
            {
                restRequestGrid.Items.Add(notification.RestRequest);
                RestRequestNotificationService.ChangeActiveStatus(notification);
            }
            appointmentGrid.Items.Refresh();
            restRequestGrid.Items.Refresh();
            DoctorService.DeleteNotifications(_loggedDoctor);
        }
    }
}
