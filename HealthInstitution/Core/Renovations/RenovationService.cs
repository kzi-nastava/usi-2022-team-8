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
    public class RenovationService
    {
        private RenovationRepository _renovationRepository;
        public RenovationService()
        {
            _renovationRepository = RenovationRepository.GetInstance();
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
