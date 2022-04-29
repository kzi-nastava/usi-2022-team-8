using HealthInstitution.Core.Equipments.Model;

namespace HealthInstitution.Core.Rooms.Model;

public class Room
{
    public RoomType type { get; set; }
    public int number { get; set; }
    public bool isRenovating { get; set; }
    public List<Equipment> availableEquipment { get; set; }
    public List<DateTime> scheduledAppointments { get; set; }

    public Room(RoomType type, int number, bool isRenovating, List<Equipment> availableEquipment, List<DateTime> scheduledAppointments)
    {
        this.type = type;
        this.number = number;
        this.isRenovating = isRenovating;
        this.availableEquipment = availableEquipment;
        this.scheduledAppointments = scheduledAppointments;
    }
}



public enum RoomType
{
    OperatingRoom,
    ExaminationRoom,
    RestRoom,
    Warehouse
}