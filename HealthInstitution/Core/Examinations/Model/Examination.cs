using HealthInstitution.Core.Appointments.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Examinations.Model;

public class Examination
{

    public int id { get; set; }
    public Appointment appointment { get; set; }
    public ExaminationStatus status { get; set; }
    public Room room { get; set; }
    public Doctor doctor { get; set; }
    public MedicalRecord medicalRecord { get; set; }
    public String anamnesis { get; set; }

    public Examination(int id, Appointment appointment, Room room, Doctor doctor, MedicalRecord medicalRecord)
    {
        this.id = id;
        this.appointment = appointment;
        this.status = ExaminationStatus.Scheduled;
        this.room = room;
        this.doctor = doctor;
        this.medicalRecord = medicalRecord;
    }
}

public enum ExaminationStatus
{
    Scheduled,
    Canceled,
    Completed
}