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
    internal class EquipmentService
    {
        private EquipmentService()
        {
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
    }
}
