using zdravstvenaUstanova.Core.Equipments.Model;

namespace zdravstvenaUstanova.Core.Rooms.Model;

public class Room
{
    public RoomType type { get; set; }
    public int number { get; set; }
    public bool isRenovating { get; set; }
    public List<Equipment> availableEquipments { get; set; }
    
    
}

public enum RoomType
{
    OperatingRoom,
    ExaminationRoom,
    RestRoom,
    Warehouse
}