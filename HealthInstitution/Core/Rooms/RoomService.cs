﻿using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Renovations;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.GUI.ManagerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Rooms
{
    public static class RoomService
    {
        private static RoomRepository s_roomRepository = RoomRepository.GetInstance();

        public static List<Room> GetAll()
        {
            return s_roomRepository.GetAll();
        }

        public static Room AddRoom(RoomDTO roomDTO)
        {
            Room room = new Room(roomDTO);
            return s_roomRepository.AddRoom(room);
        }

        public static void Update(int id, RoomDTO roomDTO)
        {
            Room room = new Room(roomDTO);
            s_roomRepository.Update(id, room);
        }

        public static void Delete(int id)
        {
            s_roomRepository.Delete(id);
        }

        public static void WriteIn()
        {
            s_roomRepository.Save();
        }
        public static bool CheckImportantOccurrenceOfRoom(Room room)
        {
            if (EquipmentTransferService.CheckOccurrenceOfRoom(room))
            {
                return true;
            }

            if (SchedulingService.CheckOccurrenceOfRoom(room))
            {
                return true;
            }

            return false;
        }

        public static void MoveRoomToRenovationHistory(Room selectedRoom)
        {
            if (RenovationService.CheckRenovationStatusForHistoryDelete(selectedRoom))
            {
                s_roomRepository.Delete(selectedRoom.Id);
            }
        }

        public static bool ExistsChangedRoomNumber(int number, Room oldRoom)
        {
            int idx = s_roomRepository.FindIndexWithRoomNumber(number);
            if (idx >= 0)
            {
                if (s_roomRepository.GetAll()[idx] != oldRoom)
                {
                    return true;
                }
            }
            return false;
        }
        public static void AddToRoom(int id, Equipment equipment)
        {
            s_roomRepository.AddToRoom(id, equipment);
        }

        public static List<Room> GetActive()
        {
            return s_roomRepository.GetActive();
        }

        public static List<Room> GetNotRenovating()
        {
            return s_roomRepository.GetNotRenovating();
        }

        public static List<Equipment> GetDynamicEquipment(Room room)
        {
            return s_roomRepository.GetDynamicEquipment(room);
        }

        public static bool RoomNumberIsTaken(int number)
        {
            return s_roomRepository.RoomNumberIsTaken(number);
        }

        public static List<Equipment> GetAvailableEquipment(Room room)
        {
            return room.AvailableEquipment;
        }

        public static List<TableItemEquipment> GetTableItemEquipments()
        {
            List<TableItemEquipment> items = new List<TableItemEquipment>();

            foreach (Room room in s_roomRepository.GetActive())
            {
                foreach (Equipment equipment in room.AvailableEquipment)
                {
                    TableItemEquipment equipmentByRoom = new TableItemEquipment(room, equipment);
                    items.Add(equipmentByRoom);
                }
            }
            return items;
        }

        public static void UpdateEquipmentQuantity(Room room, Equipment equipment)
        { 
            int index = room.AvailableEquipment.FindIndex(eq => eq.Name == equipment.Name && eq.Type == equipment.Type);
            if (index >= 0)
            {
                room.AvailableEquipment[index].Quantity += equipment.Quantity;
                EquipmentService.Delete(equipment.Id);
            }
            else
            {
                room.AvailableEquipment.Add(equipment);
            }          
        }
        public static void RemoveEquipmentFrom(Room room)
        {
            List<Equipment> equipments = room.AvailableEquipment;
            foreach (Equipment equipment in equipments)
            {
                EquipmentService.Delete(equipment.Id);
            }
            equipments.Clear();
        }
        public static Room? GetRoomFromString(string? roomFromForm)
        {
            return s_roomRepository.GetRoomFromString(roomFromForm);
        }
    }
}
