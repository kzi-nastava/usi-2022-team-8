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
    public interface IEquipmentService : IService<EquipmentService>
    {
        public Task<IEnumerable<Equipment>> GetAll();
        public Task<Equipment> Add(EquipmentDTO equipmentDTO);
        public void Update(int id, EquipmentDTO equipmentDTO);
        public void Delete(int id);
        public Task<IEnumerable<TableItemEquipment>> FilterEquipment(EquipmentFilterDTO equipmentFilter);
        public bool MatchQuantityFilter(Equipment equipment, EquipmentFilterDTO equipmentFilter);
        public bool MatchEquipmentTypeFilter(Equipment equipment, EquipmentFilterDTO equipmentFilter);
        public bool MatchRoomTypeFilter(Room room, EquipmentFilterDTO equipmentFilter);
        public Task<IEnumerable<TableItemEquipment>> SearchEquipment(string searchInput);
        public bool SearchMatch(Room room, Equipment equipment, string searchInput);
        public int GetQuantityFromForm(string quantityFromForm, Room room, string equipmentName);
        public dynamic MakePair(Room room, Equipment equipment, int quantityInRoom);
        public int GetQuantityOfEquipmentInRoom(Room room, Equipment equipment);
        public void CheckRoomEquipmentPair(Room room, Equipment equipment, HashSet<String> distinctEquipments, List<dynamic> pairs);
        public Equipment GetEquipmentFromRoom(Room room, string equipmentName);
        public Task<IEnumerable<dynamic>> GetMissingEquipment();


    }
}
