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
    public class EquipmentService
    {
        private EquipmentRepository _equipmentRepository;

        public EquipmentService()
        {
            _equipmentRepository = EquipmentRepository.GetInstance();
        }

        public static List<TableItemEquipment> FilterEquipment(EquipmentFilterDTO equipmentFilter)
        {
            RoomRepository roomRepository = RoomRepository.GetInstance();

            List<TableItemEquipment> items = new List<TableItemEquipment>();
            List<Room> rooms = roomRepository.GetActive();
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
    }
}
