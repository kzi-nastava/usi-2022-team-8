using HealthInstitution.Core.Equipments.Model;

namespace HealthInstitution.Core.Operations.Model;

public class EquipmentCost
{
    public Operation Operation { get; set; }
    public Equipment Equipment { get; set; }
    public int Quantity { get; set; }


    public EquipmentCost(Operation operation, Equipment equipment, int quantity)
    {
        this.Operation = operation;
        this.Equipment = equipment;
        this.Quantity = quantity;
    }
}