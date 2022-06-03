using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.GUI.ManagerView;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Equipments
{
    internal static class EquipmentService
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
        public static Equipment GetEquipmentFromRoom(Room room, string equipmentName)
        {
            foreach (Equipment equipment in room.AvailableEquipment)
                if (equipment.Name == equipmentName)
                    return equipment;
            return null;
        }
        public static int GetQuantityFromForm(string quantityFromForm, Room room, string equipmentName)
        {
            int quantity;
            string exceptionMessage = "Quantity must be filled";
            try
            {
                quantity = int.Parse(quantityFromForm);
                Equipment equipment = GetEquipmentFromRoom(room, equipmentName);
                if (quantity + 5 > equipment.Quantity)
                {
                    exceptionMessage = "You can't transfer this quantity of equipment. There must be at least 5 items left in the room.";
                    throw new Exception();
                }
                return quantity;
            }
            catch
            {
                throw new Exception(exceptionMessage);
            }
        }
        private static dynamic MakePair(Room room, Equipment equipment, int quantityInRoom)
        {
            dynamic obj = new ExpandoObject();
            obj.Room = room;
            obj.Equipment = equipment.Name;
            obj.Quantity = quantityInRoom;
            return obj;
        }
        private static int GetQuantityOfEquipmentInRoom(Room room, Equipment equipment)
        {
            int quantity = 0;
            foreach (Equipment e in room.AvailableEquipment)
            {
                if (e.Name == equipment.Name)
                    quantity += e.Quantity;
            }
            return quantity;
        }
        private static void CheckRoomEquipmentPair(Room room, Equipment equipment, HashSet<String> distinctEquipments, List<dynamic> pairs)
        {
            if (equipment.IsDynamic && !distinctEquipments.Contains(equipment.Name))
            {
                distinctEquipments.Add(equipment.Name);
                int quantityInRoom = GetQuantityOfEquipmentInRoom(room, equipment);
                if (room.Id != 1 && quantityInRoom < 5)
                {
                    pairs.Add(MakePair(room, equipment, quantityInRoom));
                }
            }
        }
        public static List<dynamic> GetMissingEquipment()
        {
            List<Equipment> equipments = EquipmentRepository.GetInstance().Equipments;
            List<Room> rooms = RoomRepository.GetInstance().Rooms;
            List<dynamic> pairs = new();
            HashSet<string> distinctEquipments;
            foreach (Room room in rooms)
            {
                distinctEquipments = new();
                foreach (Equipment equipment in equipments)
                {
                    CheckRoomEquipmentPair(room, equipment, distinctEquipments, pairs);
                }
            }
            return pairs;
        }

        public static void RemoveConsumed(Equipment equipment, int consumedQuantity)
        {
            s_equipmentRepository
        }
}
