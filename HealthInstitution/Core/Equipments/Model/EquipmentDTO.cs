using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Equipments.Model
{
    public class EquipmentDTO
    {
        public int Quantity { get; set; }
        public String Name { get; set; }
        public EquipmentType Type { get; set; }
        public bool IsDynamic { get; set; }

        public EquipmentDTO(int quantity, string name, EquipmentType type, bool isDynamic)
        {
            this.Quantity = quantity;
            this.Name = name;
            this.Type = type;
            this.IsDynamic = isDynamic;
        }
    }

    
}
