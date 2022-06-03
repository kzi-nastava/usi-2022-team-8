using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Examinations.Model;

public class Examination
{
    public int Id { get; set; }
    public ExaminationStatus Status { get; set; }
    public DateTime Appointment { get; set; }
    public Room Room { get; set; }
    public Doctor Doctor { get; set; }
    public MedicalRecord MedicalRecord { get; set; }
    public String Anamnesis { get; set; }

    public Examination(int id, ExaminationStatus status, DateTime appointment, Room room, Doctor doctor, MedicalRecord medicalRecord, String anamnesis)
    {
        this.Id = id;
        this.Status = status;
        this.Appointment = appointment;
        this.Room = room;
        this.Doctor = doctor;
        this.MedicalRecord = medicalRecord;
        this.Anamnesis = anamnesis;
    }

    public Examination(ExaminationDTO examinationDTO)
    {
        this.Status = ExaminationStatus.Scheduled;
        this.Appointment = examinationDTO.Appointment;
        this.Room = examinationDTO.Room;
        this.Doctor = examinationDTO.Doctor;
        this.MedicalRecord = examinationDTO.MedicalRecord;
        this.Anamnesis = "";
    }
}

public enum ExaminationStatus
{
    Scheduled,
    Canceled,
    Completed
}