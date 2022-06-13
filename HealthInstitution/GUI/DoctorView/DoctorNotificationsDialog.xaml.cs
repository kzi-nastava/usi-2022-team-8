using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Notifications.Repository;
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
        IDoctorService _doctorService;
        IAppointmentNotificationService _appointmentNotificationService;
        public DoctorNotificationsDialog(Doctor doctor)
        {
            _loggedDoctor = doctor;
            InitializeComponent();
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<AppointmentNotification> doctorsNotifications = _loggedDoctor.Notifications;
            foreach (AppointmentNotification notification in doctorsNotifications)
            {
                dataGrid.Items.Add(notification);
                AppointmentNotificationService.ChangeActiveStatus(notification,true);
            }
            dataGrid.Items.Refresh();
            DoctorService.DeleteNotifications(_loggedDoctor);
        }
    }
}
