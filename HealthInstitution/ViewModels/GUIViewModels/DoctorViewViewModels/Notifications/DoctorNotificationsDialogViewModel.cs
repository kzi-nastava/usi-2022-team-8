using HealthInstitution.Core;
using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.ViewModels.ModelViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Notifications
{
    public class DoctorNotificationsDialogViewModel : ViewModelBase
    {
        public List<AppointmentNotification> Notifications;

        private ObservableCollection<AppointmentNotificationViewModel> _notificationsVM;

        public ObservableCollection<AppointmentNotificationViewModel> NotificationsVM
        {
            get
            {
                return _notificationsVM;
            }
            set
            {
                _notificationsVM = value;
                OnPropertyChanged(nameof(NotificationsVM));
            }
        }
        public DoctorNotificationsDialogViewModel(Doctor doctor)
        {
            Notifications = new();
            Notifications = doctor.Notifications;
            _notificationsVM = new();
            LoadRows(doctor);
        }

        private void LoadRows(Doctor doctor)
        {
            foreach (var notification in Notifications)
            {
                _notificationsVM.Add(new AppointmentNotificationViewModel(notification));
                AppointmentNotificationService.ChangeActiveStatus(notification, true);
            }
            DoctorService.DeleteNotifications(doctor);
        }  }
}
