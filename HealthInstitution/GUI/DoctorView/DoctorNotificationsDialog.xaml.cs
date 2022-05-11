using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Notifications.Repository;
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
        private DoctorRepository _doctorRepository;
        private NotificationRepository _notificationRepository;
        public DoctorNotificationsDialog(Doctor doctor)
        {
            _loggedDoctor = doctor;
            _doctorRepository = DoctorRepository.GetInstance();
            _notificationRepository = NotificationRepository.GetInstance();
            InitializeComponent();
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Notification> doctorsNotifications = _loggedDoctor.Notifications;
            //List<Notification> doctorsNotificationsCopy = doctorsNotifications.ConvertAll(notification => new Notification(notification.Id,notification.OldAppointment,notification.NewAppointment,notification.Doctor,notification.Patient,notification.ActiveForDoctor,notification.ActiveForPatient));
            foreach (Notification notification in doctorsNotifications)
            {
                dataGrid.Items.Add(notification);
                notification.ActiveForDoctor = false;
                NotificationRepository.GetInstance().Save();
                //_notificationRepository.Delete(notification.Id);
                //_doctorRepository.DeleteNotification(_loggedDoctor, notification);
            }
            dataGrid.Items.Refresh();
        }
    }
}
