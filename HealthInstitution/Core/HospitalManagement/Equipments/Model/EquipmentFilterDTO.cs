using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Equipments.Model
{
    public class EquipmentFilterDTO
    {
        public bool ApplyRoomTypeFilter { get; set; }
        public RoomType RoomTypeFilter { get; set; }

        public bool ApplyEquipmentTypeFilter { get; set; }
        public EquipmentType EquipmentTypeFilter { get; set; }

        public bool ApplyQuantityFilter { get; set; }
        public int QuantityFilter { get; set; }

        public EquipmentFilterDTO(bool applyRoomTypeFilter, RoomType roomTypeFilter, bool applyEquipmentTypeFilter, EquipmentType equipmentTypeFilter, bool applyQuantityFilter, int quantityFilter)
        {
            ApplyRoomTypeFilter = applyRoomTypeFilter;
            RoomTypeFilter = roomTypeFilter;
            ApplyEquipmentTypeFilter = applyEquipmentTypeFilter;
            EquipmentTypeFilter = equipmentTypeFilter;
            ApplyQuantityFilter = applyQuantityFilter;
            QuantityFilter = quantityFilter;
        }
    }
}
