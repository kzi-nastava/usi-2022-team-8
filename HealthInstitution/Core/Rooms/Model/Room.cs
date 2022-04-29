using HealthInstitution.Core.Equipments.Model;

namespace HealthInstitution.Core.Rooms.Model;

public class Room
{
    public int id { get; set; }
    public RoomType type { get; set; }
    public int number { get; set; }
    public bool isRenovating { get; set; }
    public List<Equipment> availableEquipment { get; set; }

    public Room(int id, RoomType type, int number, bool isRenovating, List<Equipment> availableEquipment)
    {
        this.id = id;
        this.type = type;
        this.number = number;
        this.isRenovating = isRenovating;
        this.availableEquipment = availableEquipment;
    }
}

public enum RoomType
{
    OperatingRoom,
    ExaminationRoom,
    RestRoom,
    Warehouse
}