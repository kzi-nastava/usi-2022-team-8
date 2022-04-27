using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Operations.Model;

public class Operation
{
    public DateTime startTime { get; set; }
    public int duration { get; set; }
    public ExaminationStatus status { get; set; }
    public Room room { get; set; }
    public Doctor doctor { get; set; }
    public MedicalRecord medicalRecord { get; set; }
    public String report { get; set; }
    public List<EquipmentCost> equipmentCosts { get; set; }
}