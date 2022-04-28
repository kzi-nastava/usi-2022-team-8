using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.EquipmentTransfers.Model;

public class EquipmentTransfer
{
    public int id { get; set; }
    public Equipment equipment { get; set; }
    public Room fromRoom { get; set; }
    public Room toRoom { get; set; }
    public DateTime transferTime { get; set; }

    public EquipmentTransfer(int id, Equipment equipment, Room fromRoom, Room toRoom, DateTime transferTime)
    {
        this.id = id;
        this.equipment = equipment;
        this.fromRoom = fromRoom;
        this.toRoom = toRoom;
        this.transferTime = transferTime;
    }
}