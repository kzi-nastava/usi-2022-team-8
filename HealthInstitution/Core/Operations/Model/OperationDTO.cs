using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Operations.Model
{
    public class OperationDTO
    {
        public DateTime Appointment { get; set; }
        public int Duration { get; set; }
        public Room? Room { get; set; }
        public Doctor? Doctor { get; set; }
        public MedicalRecord? MedicalRecord { get; set; }

        public OperationDTO(DateTime appointment, int duration, Room? room, Doctor? doctor, MedicalRecord? medicalRecord)
        {
            this.Appointment = appointment;
            this.Duration = duration;
            this.Room = room;
            this.Doctor = doctor;
            this.MedicalRecord = medicalRecord;
        }
    }
}
