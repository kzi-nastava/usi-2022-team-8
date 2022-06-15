using HealthInstitution.Core;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.ViewModels.ModelViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;

public class PatientNotificationDialogViewModel : ViewModelBase
{
    private ObservableCollection<AppointmentNotificationViewModel> _notificationVMs;

    public ObservableCollection<AppointmentNotificationViewModel> NotificationVMs
    {
        get
        {
            return _notificationVMs;
        }
        set
        {
            _notificationVMs = value;
            OnPropertyChanged(nameof(NotificationVMs));
        }
    }

    private List<AppointmentNotification> _notifications;
    private Patient _loggedPatient;

    public PatientNotificationDialogViewModel(Patient loggedPatient)
    {
        _loggedPatient = loggedPatient;
        List<AppointmentNotification> _Notifications = _loggedPatient.Notifications;
        PatientService.DeleteNotifications(_loggedPatient);
    }

    public void PutIntoGrid()
    {
        _notificationVMs.Clear();
        foreach (AppointmentNotification notification in _notifications)
        {
            _notificationVMs.Add(new AppointmentNotificationViewModel(notification));
        }
    }
}