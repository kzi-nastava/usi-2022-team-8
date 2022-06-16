using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.ModelViewModels;

public class AppointmentNotificationViewModel : ViewModelBase
{
    private AppointmentNotification _notification;

    public AppointmentNotificationViewModel(AppointmentNotification notification)
    {
        _notification = notification;
    }

    public DateTime? OldAppointment => _notification.OldAppointment;
    public DateTime? NewAppointment => _notification.NewAppointment;
    public string Doctor => _notification.Doctor.Name + " " + _notification.Doctor.Surname;
    public string Patient => _notification.Patient.Name + " " + _notification.Patient.Surname;
}