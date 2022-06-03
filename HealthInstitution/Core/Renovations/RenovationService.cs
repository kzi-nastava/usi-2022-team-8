using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Renovations.Repository;
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


        public static bool CheckRenovationStatusForHistoryDelete(Room room)
        {
            RenovationRepository renovationRepository = RenovationRepository.GetInstance();
            foreach (Renovation renovation in renovationRepository.Renovations)
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
            RenovationRepository renovationRepository = RenovationRepository.GetInstance();
            foreach (Renovation renovation in renovationRepository.Renovations)
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
