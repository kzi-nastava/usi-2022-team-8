using System.Text.Json.Serialization;

namespace HealthInstitution.Core.Equipments.Model;

public class Equipment
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public String Name { get; set; }
    public EquipmentType Type { get; set; }
    public bool IsDynamic { get; set; }

    [JsonConstructor]
    public Equipment(int id, int quantity, string name, EquipmentType type, bool isDynamic)
    {
        this.Id = id;
        this.Quantity = quantity;
        this.Name = name;
        this.Type = type;
        this.IsDynamic = isDynamic;
    }

    public Equipment(EquipmentDTO equipmentDTO)
    {
        this.Quantity = equipmentDTO.Quantity;
        this.Name = equipmentDTO.Name;
        this.Type = equipmentDTO.Type;
        this.IsDynamic = equipmentDTO.IsDynamic;
    }

    public override string? ToString()
    {
        return Name+" (has "+Quantity+")";
    }

    public bool HasEquipmentType(EquipmentType equipmentType)
    {
        return this.Type == equipmentType;
    }
}

public enum EquipmentType
{
    AppointmentEquipment,
    SurgeryEquipment,
    RoomFurniture,
    HallEquipment
}