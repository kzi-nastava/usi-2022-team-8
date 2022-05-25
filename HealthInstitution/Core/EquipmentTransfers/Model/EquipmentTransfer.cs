using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.EquipmentTransfers.Model;

public class EquipmentTransfer
{
    public int Id { get; set; }
    public Equipment Equipment { get; set; }
    public Room? FromRoom { get; set; }
    public Room ToRoom { get; set; }
    public DateTime TransferTime { get; set; }

    public EquipmentTransfer(int id, Equipment equipment, Room? fromRoom, Room toRoom, DateTime transferTime)
    {
        this.Id = id;
        this.Equipment = equipment;
        this.FromRoom = fromRoom;
        this.ToRoom = toRoom;
        this.TransferTime = transferTime;
    }
}