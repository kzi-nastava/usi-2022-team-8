using HealthInstitution.Core.Equipments.Model;

namespace HealthInstitution.Core.Operations.Model;

public class EquipmentCost
{
    public Operation operation { get; set; }
    public Equipment equipment { get; set; }
    public int quantity { get; set; }

    public EquipmentCost(Operation operation, Equipment equipment, int quantity)
    {
        this.operation = operation;
        this.equipment = equipment;
        this.quantity = quantity;
    }
}