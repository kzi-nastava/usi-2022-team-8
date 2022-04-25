using HealthInstitution.Core.Appointments.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Examinations.Model;

public class Examination
{
    public Appointment appointment { get; set; }
    public ExaminationStatus status { get; set; }
    public Room room { get; set; }
    public Doctor doctor { get; set; }
    public MedicalRecord medicalRecord { get; set; }
    public String anamnesis { get; set; }
}

public enum ExaminationStatus
{
    Scheduled,
    Canceled,
    Completed
}