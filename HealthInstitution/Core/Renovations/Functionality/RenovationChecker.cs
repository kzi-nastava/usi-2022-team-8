using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Renovations.Repository;
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

        public static void UpdateByRenovation()
        {
            foreach (Renovation renovation in s_renovationRepository.Renovations)
            {

                if (renovation.GetType() == typeof(Renovation))
                {
                    if (renovation.StartDate == DateTime.Today)
                    {
                        StartRenovation(renovation.Room);
                    }

                    if (renovation.EndDate == DateTime.Today.AddDays(-1))
                    {
                        EndRenovation(renovation.Room);
                    }
                }
                else if (renovation.GetType() == typeof(RoomMerger))
                {
                    RoomMerger roomMerger = (RoomMerger)renovation;
                    //todo
                }
                else
                {
                    RoomSeparation roomSeparation = (RoomSeparation)renovation;
                    //todo
                }
            }
        }

        public static void StartRenovation(Room room)
        {
            s_roomRepository.Update(room.Id, room.Type, room.Number, true);
        }

        public static void EndRenovation(Room room)
        {
            s_roomRepository.Update(room.Id, room.Type, room.Number, false);
        }
    }
}
