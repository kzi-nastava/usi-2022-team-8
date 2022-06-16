using HealthInstitution.Core;
using HealthInstitution.Core.PrescriptionNotifications.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using System.Collections.ObjectModel;
using HealthInstitution.ViewModels.ModelViewModels;

namespace HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.PrescriptionNotificationViewModels;

public class PrescriptionNotificationDialogViewModel : ViewModelBase
{

    private List<PrescriptionNotification> _notifications;
    private Patient _loggedPatient;
    IPrescriptionNotificationService _prescriptionNotificationService;
    

    public PrescriptionNotificationDialogViewModel(Patient loggedPatient, IPrescriptionNotificationService prescriptionNotificationService)
    {
        _prescriptionNotificationService = prescriptionNotificationService;
        _loggedPatient = loggedPatient;
        _notifications = _prescriptionNotificationService.GetPatientActiveNotification(_loggedPatient.Username);
        _notificationVMs = new();
        PutIntoGrid();
    }


    private ObservableCollection<PrescriptionNotificationViewModel> _notificationVMs;
    public ObservableCollection<PrescriptionNotificationViewModel> NotificationVMs
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
        foreach (PrescriptionNotification notification in _notifications)
        {
            _notificationVMs.Add(new PrescriptionNotificationViewModel(notification));
        }
    }
}