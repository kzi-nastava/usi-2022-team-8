using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Renovations.Repository;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Renovations
{
    public static class RenovationService
    {
        private static RenovationRepository s_renovationRepository = RenovationRepository.GetInstance();
        private static OperationRepository s_operationRepository = OperationRepository.GetInstance();
        private static ExaminationRepository s_examinationRepository = ExaminationRepository.GetInstance();
        public static List<Renovation> GetAll()
        {
            return s_renovationRepository.GetAll();
        }

        public static void AddRenovation(RenovationDTO renovationDTO)
        {
            Renovation renovation = new Renovation(renovationDTO);
            s_renovationRepository.AddRenovation(renovation);
        }
        public static void AddRoomMerger(RoomMergerDTO roomMergerDTO)
        {
            Renovation renovation = new RoomMerger(roomMergerDTO);
            s_renovationRepository.AddRenovation(renovation);
        }
        
        public static void AddRoomSeparation(RoomSeparationDTO roomSeparationDTO)
        {
            Renovation renovation = new RoomSeparation(roomSeparationDTO);
            s_renovationRepository.AddRenovation(renovation);
        }

        public static void UpdateRenovation(int id, RenovationDTO renovationDTO)
        {
            Renovation renovation = new Renovation(renovationDTO);
            s_renovationRepository.UpdateRenovation(id, renovation);
        }
        public static void UpdateRoomMerger(int id, RoomMergerDTO roomMergerDTO)
        {
            RoomMerger roomMerger = new RoomMerger(roomMergerDTO);
            s_renovationRepository.UpdateRoomMerger(id, roomMerger);
        }
        public static void UpdateRoomSeparation(int id, RoomSeparationDTO roomSeparationDTO)
        {
            RoomSeparation roomSeparation = new RoomSeparation(roomSeparationDTO);
            s_renovationRepository.UpdateRoomSeparation(id, roomSeparation);
        }
        public static void Delete(int id)
        {
            s_renovationRepository.Delete(id);
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
                RoomService.UpdateEquipmentQuantity(mergedRoom, equipment);
            }
            secondRoom.AvailableEquipment.Clear();

            RoomDTO mergedRoomDTO = new RoomDTO(mergedRoom.Type, mergedRoom.Number, false, true);
            RoomService.Update(mergedRoom.Id, mergedRoomDTO);
            RoomDTO firstRoomDTO = new RoomDTO(firstRoom.Type, firstRoom.Number, false, false);
            RoomService.Update(firstRoom.Id, firstRoomDTO);
            RoomDTO secondRoomDTO = new RoomDTO(secondRoom.Type, secondRoom.Number, false, false);
            RoomService.Update(secondRoom.Id, secondRoomDTO);
        }

        public static void StartSeparation(Room separationRoom, Room firstRoom, Room secondRoom)
        {
            RoomDTO separationRoomDTO = new RoomDTO(separationRoom.Type, separationRoom.Number, true);
            RoomService.Update(separationRoom.Id, separationRoomDTO);
        }

        public static void EndSeparation(Room separationRoom, Room firstRoom, Room secondRoom)
        {
            RoomService.RemoveEquipmentFrom(separationRoom);

            RoomDTO separationRoomDTO = new RoomDTO(separationRoom.Type, separationRoom.Number, false, false);
            RoomService.Update(separationRoom.Id, separationRoomDTO);
            RoomDTO firstRoomDTO = new RoomDTO(firstRoom.Type, firstRoom.Number, false, true);
            RoomService.Update(firstRoom.Id, firstRoomDTO);
            RoomDTO secondRoomDTO = new RoomDTO(secondRoom.Type, secondRoom.Number, false, true);
            RoomService.Update(secondRoom.Id, secondRoomDTO);
        }


        public static bool CheckRenovationStatusForHistoryDelete(Room room)
        {
            foreach (Renovation renovation in s_renovationRepository.Renovations)
            {
                if (renovation.Room == room)
                {
                    room.IsActive = false;
                    return false;
                }
                if (renovation.IsRoomMerger())
                {
                    RoomMerger roomMerger = (RoomMerger)renovation;
                    if (roomMerger.RoomForMerge == room)
                    {
                        room.IsActive = false;
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool CheckRenovationStatusForRoom(Room room, DateTime date)
        {
            foreach (Renovation renovation in s_renovationRepository.Renovations)
            {
                if (renovation.StartDate > date && renovation.IsRoomSeparation())
                {
                    continue;
                }
                if (renovation.Room == room)
                {
                    return false;
                }

                if (renovation.IsRoomMerger())
                {
                    RoomMerger merger = (RoomMerger)renovation;
                    if (merger.RoomForMerge == room)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }


}
