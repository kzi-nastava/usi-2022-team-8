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
        public RenovationRefreshingService(IRenovationRepository renovationRepository, IRenovationService renovationService)
        {
            _renovationRepository = renovationRepository;
            _renovationService = renovationService;
        }
        public void UpdateByRenovation()
        {
            foreach (Renovation renovation in _renovationRepository.GetAll())
            {
                if (renovation.HasActiveRooms())
                {
                    Update(renovation);
                }
            }
        }
        private void Update(Renovation renovation)
        {
            if (renovation.ShouldStart())
            {
                _renovationService.Start(renovation);
            }

            if (renovation.ShouldEnd())
            {
                _renovationService.End(renovation);
            }
        }

    }
}
