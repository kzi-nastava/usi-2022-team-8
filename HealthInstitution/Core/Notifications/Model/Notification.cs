using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Notifications.Model;

public class Notification
{
    public int Id { get; set; }
    public DateTime OldAppointment { get; set; }
    public DateTime NewAppointment { get; set; }
    public Doctor Doctor { get; set; }
    public Patient Patient { get; set; }

    public Notification(int id, DateTime oldAppointment, DateTime newAppointment, Doctor doctor, Patient patient)
    {
        Id = id;
        OldAppointment = oldAppointment;
        NewAppointment = newAppointment;
        Doctor = doctor;
        Patient = patient;
    }
}
