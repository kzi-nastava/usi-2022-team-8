using HealthInstitution.Core.Equipments.Model;

namespace HealthInstitution.Core.Rooms.Model;

public class Room
{
    public int Id { get; set; }
    public RoomType Type { get; set; }
    public int Number { get; set; }
    public bool IsRenovating { get; set; }
    public bool IsActive { get; set; }
    public List<Equipment> AvailableEquipment { get; set; }

    public Room(int id, RoomType type, int number, bool isRenovating, List<Equipment> availableEquipment)
    {
        this.Id = id;
        this.Type = type;
        this.Number = number;
        this.IsRenovating = isRenovating;
        this.AvailableEquipment = availableEquipment;
        this.IsActive = true;
    }

    public Room(int id, RoomType type, int number, bool isRenovating, List<Equipment> availableEquipment, bool isActive)
    {
        this.Id = id;
        this.Type = type;
        this.Number = number;
        this.IsRenovating = isRenovating;
        this.AvailableEquipment = availableEquipment;
        this.IsActive = isActive;
    }

    public override string? ToString()
    {
        return Type.ToString()+" "+Number;
    }
    /*public string? ToString(int availableEquipment)
    {
        return Type.ToString() + " " + Number + " (has " + availableEquipment + ")";
    }*/
}

public enum RoomType
{
    OperatingRoom,
    ExaminationRoom,
    RestRoom,
    Warehouse
}