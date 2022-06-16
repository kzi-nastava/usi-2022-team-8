using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public class AppointmentType
    {
        public int Id { get; set; }
        public ExaminationStatus Status { get; set; }
        public DateTime Appointment { get; set; }
        public Room Room { get; set; }
        public Doctor Doctor { get; set; }
        public MedicalRecord MedicalRecord { get; set; }
        public String Anamnesis { get; set; }

        public AppointmentType(int id, ExaminationStatus status, DateTime appointment, Room room, Doctor doctor, MedicalRecord medicalRecord, String anamnesis)
        {
            this.Id = id;
            this.Status = status;
            this.Appointment = appointment;
            this.Room = room;
            this.Doctor = doctor;
            this.MedicalRecord = medicalRecord;
            this.Anamnesis = anamnesis;
        }
    }
}
