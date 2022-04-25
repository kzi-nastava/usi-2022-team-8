namespace HealthInstitution.Core.Equipments.Model;

public class Equipment
{
    public int quantity { get; set; }
    public String name { get; set; }
    public EquipmentType type { get; set; }
    public bool isDynamic { get; set; }

    public Equipment(int quantity, string name, EquipmentType type, bool isDynamic)
    {
        this.quantity = quantity;
        this.name = name;
        this.type = type;
        this.isDynamic = isDynamic;
    }
}

public enum EquipmentType
{
    AppointmentEquipment,
    SurgeryEquipment,
    RoomFurniture,
    HallEquipment
}