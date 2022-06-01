using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Renovations.Repository;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Renovations.Functionality
{
    public class RenovationChecker
    {
        private static RenovationRepository s_renovationRepository = RenovationRepository.GetInstance();
        private static RoomRepository s_roomRepository = RoomRepository.GetInstance();
        private static EquipmentRepository s_equipmentRepository = EquipmentRepository.GetInstance();

        public static void UpdateByRenovation()
        {
            foreach (Renovation renovation in s_renovationRepository.Renovations)
            {

                if (renovation.GetType() == typeof(Renovation))
                {
                    if (!renovation.Room.IsActive)
                    {
                        continue;
                    }

                    UpdateSimpleRenovation(renovation);
                }
                else if (renovation.GetType() == typeof(RoomMerger))
                {
                    RoomMerger roomMerger = (RoomMerger)renovation;

                    if (!roomMerger.Room.IsActive || !roomMerger.RoomForMerge.IsActive)
                    {
                        continue;
                    }

                    UpdateMergeRenovation(roomMerger);
                    
                }
                else
                {
                    RoomSeparation roomSeparation = (RoomSeparation)renovation;

                    if (!roomSeparation.Room.IsActive)
                    {
                        continue;
                    }

                    UpdateSeparationRenovation(roomSeparation);
                    
                }
            }
        }

        private static void UpdateSeparationRenovation(RoomSeparation roomSeparation)
        {
            if (roomSeparation.StartDate <= DateTime.Today.AddDays(-1))
            {
                StartSeparation(roomSeparation.Room, roomSeparation.FirstRoom, roomSeparation.SecondRoom);
            }

            if (roomSeparation.EndDate <= DateTime.Today.AddDays(-1))
            {
                EndSeparation(roomSeparation.Room, roomSeparation.FirstRoom, roomSeparation.SecondRoom);
            }
        }

        private static void UpdateMergeRenovation(RoomMerger roomMerger)
        {
            if (roomMerger.StartDate <= DateTime.Today.AddDays(-1))
            {
                StartMerge(roomMerger.Room, roomMerger.RoomForMerge, roomMerger.MergedRoom);
            }

            if (roomMerger.EndDate <= DateTime.Today.AddDays(-1))
            {
                EndMerge(roomMerger.Room, roomMerger.RoomForMerge, roomMerger.MergedRoom);
            }
        }

        private static void UpdateSimpleRenovation(Renovation renovation)
        {
            if (renovation.StartDate <= DateTime.Today.AddDays(-1))
            {
                StartRenovation(renovation.Room);
            }

            if (renovation.EndDate <= DateTime.Today.AddDays(-1))
            {
                EndRenovation(renovation.Room);
            }
        }

        public static void StartRenovation(Room room)
        {
            RoomDTO roomDTO = new RoomDTO(room.Type, room.Number, true);
            RoomService.Update(room.Id, roomDTO);
        }

        public static void EndRenovation(Room room)
        {
            RoomDTO roomDTO = new RoomDTO(room.Type, room.Number, false);
            RoomService.Update(room.Id, roomDTO);
        }

        public static void StartMerge(Room firstRoom, Room secondRoom, Room mergedRoom)
        {
            RoomDTO firstRoomDTO = new RoomDTO(firstRoom.Type, firstRoom.Number, true);
            RoomDTO secondRoomDTO = new RoomDTO(secondRoom.Type, secondRoom.Number, true);
            RoomService.Update(firstRoom.Id, firstRoomDTO);
            RoomService.Update(secondRoom.Id, secondRoomDTO);
        }

        public static void EndMerge(Room firstRoom, Room secondRoom, Room mergedRoom)
        {
            foreach (Equipment equipment in firstRoom.AvailableEquipment)
            {
                mergedRoom.AvailableEquipment.Add(equipment);
            }
            firstRoom.AvailableEquipment.Clear();

            foreach (Equipment equipment in secondRoom.AvailableEquipment)
            {
                UpdateQuantity(mergedRoom, equipment);
            }
            secondRoom.AvailableEquipment.Clear();

            RoomDTO mergedRoomDTO = new RoomDTO(mergedRoom.Type, mergedRoom.Number, false, true);
            RoomService.Update(mergedRoom.Id, mergedRoomDTO);
            RoomDTO firstRoomDTO = new RoomDTO(firstRoom.Type, firstRoom.Number, false, false);
            RoomService.Update(firstRoom.Id, firstRoomDTO);
            RoomDTO secondRoomDTO = new RoomDTO(secondRoom.Type, secondRoom.Number, false, false);
            RoomService.Update(secondRoom.Id, secondRoomDTO);
        }

        private static void UpdateQuantity(Room mergedRoom, Equipment equipment)
        {
            int index = mergedRoom.AvailableEquipment.FindIndex(eq => eq.Name == equipment.Name && eq.Type == equipment.Type);
            if (index >= 0)
            {
                mergedRoom.AvailableEquipment[index].Quantity += equipment.Quantity;
                EquipmentService.Delete(equipment.Id);
            }
            else
            {
                mergedRoom.AvailableEquipment.Add(equipment);
            }
        }

        public static void StartSeparation(Room separationRoom , Room firstRoom, Room secondRoom)
        {
            RoomDTO separationRoomDTO = new RoomDTO(separationRoom.Type, separationRoom.Number, true);
            RoomService.Update(separationRoom.Id, separationRoomDTO);
        }

        public static void EndSeparation(Room separationRoom, Room firstRoom, Room secondRoom)
        {
            ClearFromList(separationRoom.AvailableEquipment);

            RoomDTO separationRoomDTO = new RoomDTO(separationRoom.Type, separationRoom.Number, false, false);
            RoomService.Update(separationRoom.Id, separationRoomDTO);
            RoomDTO firstRoomDTO = new RoomDTO(firstRoom.Type, firstRoom.Number, false, true);
            RoomService.Update(firstRoom.Id, firstRoomDTO);
            RoomDTO secondRoomDTO = new RoomDTO(secondRoom.Type, secondRoom.Number, false, true);
            RoomService.Update(secondRoom.Id, secondRoomDTO);
        }

        private static void ClearFromList(List<Equipment> equipments)
        {
            foreach (Equipment equipment in equipments)
            {
                EquipmentService.Delete(equipment.Id);
            }
            equipments.Clear();
        }
    }
}
