namespace HealthInstitution.Core.Equipments.Model;

public class Equipment
{
    public int id { get; set; }
    public int quantity { get; set; }
    public String name { get; set; }
    public EquipmentType type { get; set; }
    public bool isDynamic { get; set; }

    public Equipment(int id, int quantity, string name, EquipmentType type, bool isDynamic)
    {
        this.id = id;
        this.quantity = quantity;
        this.name = name;
        this.type = type;
        this.isDynamic = isDynamic;
    }

    public override string? ToString()
    {
        return name+" (has "+quantity+")";
    }
}

public enum EquipmentType
{
    AppointmentEquipment,
    SurgeryEquipment,
    RoomFurniture,
    HallEquipment
}