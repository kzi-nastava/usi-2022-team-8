using HealthInstitution.Core;
using HealthInstitution.Core.Notifications;
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
    private List<AppointmentNotification> _notifications;
    private Patient _loggedPatient;
    IPatientService _patientService;
    IAppointmentNotificationService _appointmentNotificationService;
    public PatientNotificationDialogViewModel(Patient loggedPatient, IPatientService patientService, IAppointmentNotificationService appointmentNotificationService)
    {
        _loggedPatient = loggedPatient;
        _patientService = patientService;
        _appointmentNotificationService = appointmentNotificationService;
        _notifications = _loggedPatient.Notifications;
        _notificationVMs = new();
        PutIntoGrid();
        _patientService.DeleteNotifications(_loggedPatient);
    }

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

    

    public void PutIntoGrid()
    {
        _notificationVMs.Clear();
        foreach (AppointmentNotification notification in _notifications)
        {
            _notificationVMs.Add(new AppointmentNotificationViewModel(notification));
            _appointmentNotificationService.ChangeActiveStatus(notification, false);
        }
    }
}