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
    public static class RenovationRefreshingService
    {
        private static RenovationRepository s_renovationRepository = RenovationRepository.GetInstance();
       
        public static void UpdateByRenovation()
        {
            foreach (Renovation renovation in s_renovationRepository.Renovations)
            {

                if (renovation.IsSimpleRenovation())
                {
                    if (renovation.Room.IsActive)
                    {
                        UpdateSimpleRenovation(renovation);
                    }                   
                }
                else if (renovation.IsRoomMerger())
                {
                    RoomMerger roomMerger = (RoomMerger)renovation;

                    if (roomMerger.Room.IsActive && roomMerger.RoomForMerge.IsActive)
                    {
                        UpdateMergeRenovation(roomMerger);
                    }  
                }
                else
                {
                    RoomSeparation roomSeparation = (RoomSeparation)renovation;

                    if (roomSeparation.Room.IsActive)
                    {
                        UpdateSeparationRenovation(roomSeparation);
                    }
  
                }
            }
        }

        private static void UpdateSeparationRenovation(RoomSeparation roomSeparation)
        {
            if (roomSeparation.StartDate <= DateTime.Today.AddDays(-1))
            {
                RenovationService.StartSeparation(roomSeparation.Room, roomSeparation.FirstRoom, roomSeparation.SecondRoom);
            }

            if (roomSeparation.EndDate <= DateTime.Today.AddDays(-1))
            {
                RenovationService.EndSeparation(roomSeparation.Room, roomSeparation.FirstRoom, roomSeparation.SecondRoom);
            }
        }

        private static void UpdateMergeRenovation(RoomMerger roomMerger)
        {
            if (roomMerger.StartDate <= DateTime.Today.AddDays(-1))
            {
                RenovationService.StartMerge(roomMerger.Room, roomMerger.RoomForMerge, roomMerger.MergedRoom);
            }

            if (roomMerger.EndDate <= DateTime.Today.AddDays(-1))
            {
                RenovationService.EndMerge(roomMerger.Room, roomMerger.RoomForMerge, roomMerger.MergedRoom);
            }
        }

        private static void UpdateSimpleRenovation(Renovation renovation)
        {
            if (renovation.StartDate <= DateTime.Today.AddDays(-1))
            {
                RenovationService.StartRenovation(renovation.Room);
            }

            if (renovation.EndDate <= DateTime.Today.AddDays(-1))
            {
                RenovationService.EndRenovation(renovation.Room);
            }
        }
    }
}
