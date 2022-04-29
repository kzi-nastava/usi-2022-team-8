using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Operations.Model;

public class Operation
{
    public int id { get; set; }
    public DateTime appointment { get; set; }
    public int duration { get; set; }
    public ExaminationStatus status { get; set; }
    public Room room { get; set; }
    public Doctor doctor { get; set; }
    public MedicalRecord medicalRecord { get; set; }
    public String report { get; set; }
    public List<EquipmentCost> equipmentCosts { get; set; }

    public Operation(int id, ExaminationStatus status, DateTime appointment, int duration, Room room, Doctor doctor, MedicalRecord medicalRecord)
    {
        this.id = id;
        this.status = status;
        this.appointment = appointment;
        this.duration = duration;
        this.room = room;
        this.doctor = doctor;
        this.medicalRecord = medicalRecord;
        this.equipmentCosts = new List<EquipmentCost>();
    }
}