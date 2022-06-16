using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Notifications.Model
{
    public class AppointmentNotificationDTO
    {
        public DateTime? OldAppointment { get; set; }
        public DateTime NewAppointment { get; set; }
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }

        public AppointmentNotificationDTO(DateTime? oldAppointment, DateTime newAppointment, Doctor doctor, Patient patient)
        {
            OldAppointment = oldAppointment;
            NewAppointment = newAppointment;
            Doctor = doctor;
            Patient = patient;
        }
    }
}
