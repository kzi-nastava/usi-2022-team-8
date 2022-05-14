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

        public void Validate()
        {
            if (this.Appointment <= DateTime.Now)
                throw new Exception("You have to change dates for upcoming ones!");
            if (this.Duration <= 15)
                throw new Exception("Operation can't last less than 15 minutes!");
            if (this.MedicalRecord.Patient.Blocked != BlockState.NotBlocked)
                throw new Exception("Patient is blocked and can not have any examinations!");
        }
    }
}
