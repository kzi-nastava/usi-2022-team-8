namespace HealthInstitution.Core.Equipments.Model;

public class Equipment
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public String Name { get; set; }
    public EquipmentType Type { get; set; }
    public bool IsDynamic { get; set; }

    public Equipment(int id, int quantity, string name, EquipmentType type, bool isDynamic)
    {
        this.Id = id;
        this.Quantity = quantity;
        this.Name = name;
        this.Type = type;
        this.IsDynamic = isDynamic;
    }

    public override string? ToString()
    {
        return Name+" (has "+Quantity+")";
    }
}

public enum EquipmentType
{
    AppointmentEquipment,
    SurgeryEquipment,
    RoomFurniture,
    HallEquipment
}