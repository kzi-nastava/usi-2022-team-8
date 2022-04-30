using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Examinations.Model;

public class Examination
{

    public int id { get; set; }
    public ExaminationStatus status { get; set; }
    public DateTime appointment { get; set; }
    public Room room { get; set; }
    public Doctor doctor { get; set; }
    public MedicalRecord medicalRecord { get; set; }
    public String anamnesis { get; set; }

    public Examination(int id, ExaminationStatus status, DateTime appointment, Room room, Doctor doctor, MedicalRecord medicalRecord, String anamnesis)
    {
        this.id = id;
        this.status = status;
        this.appointment = appointment;
        this.status = ExaminationStatus.Scheduled;
        this.room = room;
        this.doctor = doctor;
        this.medicalRecord = medicalRecord;
        this.anamnesis = anamnesis;
    }
}

public enum ExaminationStatus
{
    Scheduled,
    Canceled,
    Completed
}