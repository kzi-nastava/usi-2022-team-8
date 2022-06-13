using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.GUI.ManagerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Equipments
{
    public interface IEquipmentService
    {
        public List<Equipment> GetAll();
        public Equipment Add(EquipmentDTO equipmentDTO);
        public void Update(int id, EquipmentDTO equipmentDTO);
        public void Delete(int id);
        public List<TableItemEquipment> FilterEquipment(EquipmentFilterDTO equipmentFilter);
        public List<TableItemEquipment> SearchEquipment(string searchInput);
        public Equipment GetEquipmentFromRoom(Room room, string equipmentName);
        public List<dynamic> GetMissingEquipment();
        public void RemoveConsumed(Equipment equipment, int consumedQuantity);
    }
}
