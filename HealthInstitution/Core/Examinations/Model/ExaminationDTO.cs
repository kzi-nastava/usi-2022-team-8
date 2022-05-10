using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

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
    }
}
