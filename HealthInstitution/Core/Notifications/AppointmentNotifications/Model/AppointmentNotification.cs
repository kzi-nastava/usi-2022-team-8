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

public class AppointmentNotification
{
    public int Id { get; set; }
    public DateTime? OldAppointment { get; set; }
    public DateTime NewAppointment { get; set; }
    public Doctor Doctor { get; set; }
    public Patient Patient { get; set; }

    public bool ActiveForDoctor { get; set; }   
    public bool ActiveForPatient { get; set; }

    public AppointmentNotification(int id, DateTime? oldAppointment, DateTime newAppointment, Doctor doctor, Patient patient, bool activeForDoctor, bool activeForPatient)
    {
        Id = id;
        OldAppointment = oldAppointment;
        NewAppointment = newAppointment;
        Doctor = doctor;
        Patient = patient;
        ActiveForDoctor = activeForDoctor;
        ActiveForPatient = activeForPatient;
    }
    public AppointmentNotification(AppointmentNotificationDTO appointmentNotificationDTO)
    {
        OldAppointment = appointmentNotificationDTO.OldAppointment;
        NewAppointment = appointmentNotificationDTO.NewAppointment;
        Doctor = appointmentNotificationDTO.Doctor;
        Patient = appointmentNotificationDTO.Patient;
        ActiveForDoctor = true;
        ActiveForPatient = true;
    }
}
