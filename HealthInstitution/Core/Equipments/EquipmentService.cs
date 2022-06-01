using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.GUI.ManagerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Equipments
{
    public static class EquipmentService
    {
        private static EquipmentRepository s_equipmentRepository = EquipmentRepository.GetInstance();
        private static RoomRepository s_roomRepository = RoomRepository.GetInstance();

        public static List<Equipment> GetAll()
        {
            return s_equipmentRepository.GetAll();
        }

        public static Equipment Add(EquipmentDTO equipmentDTO)
        {
            Equipment equipment = new Equipment(equipmentDTO);
            return s_equipmentRepository.Add(equipment);
        }

        public static void Update(int id, EquipmentDTO equipmentDTO)
        {
            Equipment equipment = new Equipment(equipmentDTO);
            s_equipmentRepository.Update(id, equipment);
        }
        public static void Delete(int id)
        {
            s_equipmentRepository.Delete(id);
        }

        public static List<TableItemEquipment> FilterEquipment(EquipmentFilterDTO equipmentFilter)
        {           
            List<TableItemEquipment> items = new List<TableItemEquipment>();
            List<Room> rooms = s_roomRepository.GetActive();
            foreach (Room room in rooms)
            {
                if (!MatchRoomTypeFilter(room,equipmentFilter))
                    continue;
                foreach (Equipment equipment in room.AvailableEquipment)
                {
                    if (!MatchEquipmentTypeFilter(equipment, equipmentFilter))
                        continue;
                    if (!MatchQuantityFilter(equipment, equipmentFilter))
                        continue;

                    TableItemEquipment equipmentByRoom = new TableItemEquipment(room, equipment);
                    items.Add(equipmentByRoom);
                }
            }
            return items;
        }

        private static bool MatchQuantityFilter(Equipment equipment, EquipmentFilterDTO equipmentFilter)
        {
            if (!equipmentFilter.ApplyQuantityFilter)
            {
                return true;
            }
            
            switch (equipmentFilter.QuantityFilter)
            {
                case 0:
                    if (equipment.Quantity != 0)
                        return false;
                    break;
                case 1:
                    if (equipment.Quantity > 10)
                        return false;
                    break;
                case 2:
                    if (equipment.Quantity < 10)
                        return false;
                    break;
            }
            return true;
        }

        private static bool MatchEquipmentTypeFilter(Equipment equipment, EquipmentFilterDTO equipmentFilter)
        {
            return !equipmentFilter.ApplyEquipmentTypeFilter || equipment.HasEquipmentType(equipmentFilter.EquipmentTypeFilter);
        }


        private static bool MatchRoomTypeFilter(Room room, EquipmentFilterDTO equipmentFilter)
        {
            return !equipmentFilter.ApplyRoomTypeFilter || room.HasRoomType(equipmentFilter.RoomTypeFilter);
        }

        public static List<TableItemEquipment> SearchEquipment(string searchInput)
        {
            List<TableItemEquipment> items = new List<TableItemEquipment>();

            List<Room> rooms = s_roomRepository.GetActive();
            foreach (Room room in rooms)
            {
                foreach (Equipment equipment in room.AvailableEquipment)
                {
                    if (SearchMatch(room, equipment, searchInput))
                    {
                        TableItemEquipment equipmentByRoom = new TableItemEquipment(room, equipment);
                        items.Add(equipmentByRoom);
                    }
                }
            }
            return items;
        }

        private static bool SearchMatch(Room room, Equipment equipment, string searchInput)
        {
            if (room.Type.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            if (room.Number.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            if (equipment.Type.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            if (equipment.Name.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            return false;
        }
    }
}
