using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;

namespace HealthInstitution.Core.Examinations.Model
{
    public class ExaminationDTO
    {
        public DateTime Appointment { get; set; }
        public Room? Room { get; set; }
        public Doctor? Doctor { get; set; }
        public MedicalRecord? MedicalRecord { get; set; }

        public ExaminationDTO(DateTime appointment, Room? room, Doctor? doctor, MedicalRecord? medicalRecord)
        {
            this.Appointment = appointment;
            this.Room = room;
            this.Doctor = doctor;
            this.MedicalRecord = medicalRecord;
        }

        public void Validate()
        {
            if (this.Appointment <= DateTime.Now)
                throw new Exception("You have to change dates for upcoming ones!");
            if (this.MedicalRecord.Patient.Blocked != BlockState.NotBlocked)
                throw new Exception("Patient is blocked and can not have any examinations!");
        }
    }
}
