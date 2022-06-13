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
    public class RenovationRefreshingService : IRenovationRefreshingService
    {
        IRenovationRepository _renovationRepository;
        IRenovationService _renovationService;
        public RenovationRefreshingService(IRenovationRepository renovationRepository,
            IRenovationService renovationService) 
        {
            _renovationRepository = renovationRepository;
            _renovationService = renovationService;
        }
        public void UpdateByRenovation()
        {
            foreach (Renovation renovation in _renovationRepository.GetAll())
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

        private void UpdateSeparationRenovation(RoomSeparation roomSeparation)
        {
            if (roomSeparation.StartDate <= DateTime.Today.AddDays(-1))
            {
                _renovationService.StartSeparation(roomSeparation.Room, roomSeparation.FirstRoom, roomSeparation.SecondRoom);
            }

            if (roomSeparation.EndDate <= DateTime.Today.AddDays(-1))
            {
                _renovationService.EndSeparation(roomSeparation.Room, roomSeparation.FirstRoom, roomSeparation.SecondRoom);
            }
        }

        private void UpdateMergeRenovation(RoomMerger roomMerger)
        {
            if (roomMerger.StartDate <= DateTime.Today.AddDays(-1))
            {
                _renovationService.StartMerge(roomMerger.Room, roomMerger.RoomForMerge, roomMerger.MergedRoom);
            }

            if (roomMerger.EndDate <= DateTime.Today.AddDays(-1))
            {
                _renovationService.EndMerge(roomMerger.Room, roomMerger.RoomForMerge, roomMerger.MergedRoom);
            }
        }

        private void UpdateSimpleRenovation(Renovation renovation)
        {
            if (renovation.StartDate <= DateTime.Today.AddDays(-1))
            {
                _renovationService.StartRenovation(renovation.Room);
            }

            if (renovation.EndDate <= DateTime.Today.AddDays(-1))
            {
                _renovationService.EndRenovation(renovation.Room);
            }
        }
    }
}
