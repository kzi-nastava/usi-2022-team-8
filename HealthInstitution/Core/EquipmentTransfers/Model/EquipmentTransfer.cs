using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.EquipmentTransfers.Model;

public class EquipmentTransfer
{
    public Equipment equipment { get; set; }
    public Room fromRoom { get; set; }
    public Room toRoom { get; set; }
    public DateTime transferTime { get; set; }
}