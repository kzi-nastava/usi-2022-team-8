using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.RestRequestNotifications;
using HealthInstitution.Core.RestRequestNotifications.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.ViewModels.ModelViewModels;
using HealthInstitution.ViewModels.ModelViewModels.RestRequests;
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
        public List<AppointmentNotification> AppointmentNotifications;

        private ObservableCollection<AppointmentNotificationViewModel> _appointmentNotificationsVM;

        public ObservableCollection<AppointmentNotificationViewModel> AppointmentNotificationsVM
        {
            get
            {
                return _appointmentNotificationsVM;
            }
            set
            {
                _appointmentNotificationsVM = value;
                OnPropertyChanged(nameof(AppointmentNotificationsVM));
            }
        }

        public List<RestRequestNotification> RestRequestNotifications;

        private ObservableCollection<RestRequestNotificationViewModel> _restRequestNotificationsVM;

        public ObservableCollection<RestRequestNotificationViewModel> RestRequestNotificationsVM
        {
            get
            {
                return _restRequestNotificationsVM;
            }
            set
            {
                _restRequestNotificationsVM = value;
                OnPropertyChanged(nameof(RestRequestNotificationsVM));
            }
        }

        IDoctorService _doctorService;
        IAppointmentNotificationService _appointmentNotificationService;
        IRestRequestNotificationService _restRequestNotificationService;
        public DoctorNotificationsDialogViewModel(Doctor doctor, IDoctorService doctorService, IAppointmentNotificationService appointmentNotificationService, IRestRequestNotificationService restRequestNotificationService)
        {
            _doctorService = doctorService;
            _appointmentNotificationService = appointmentNotificationService;
            _restRequestNotificationService = restRequestNotificationService;
            AppointmentNotifications = new();
            AppointmentNotifications = _doctorService.GetActiveAppointmentNotification(doctor);
            RestRequestNotifications = new();
            RestRequestNotifications = _doctorService.GetActiveRestRequestNotification(doctor);
            _appointmentNotificationsVM = new();
            _restRequestNotificationsVM = new();
            LoadRows(doctor);
        }

        private void LoadRows(Doctor doctor)
        {
            foreach (var notification in AppointmentNotifications)
            {
                _appointmentNotificationsVM.Add(new AppointmentNotificationViewModel(notification));
                _appointmentNotificationService.ChangeActiveStatus(notification, true);
            }
            foreach (var notification in RestRequestNotifications)
            {
                _restRequestNotificationsVM.Add(new RestRequestNotificationViewModel(notification));
                _restRequestNotificationService.ChangeActiveStatus(notification);
            }
            _doctorService.DeleteNotifications(doctor);
        }
    }
}
