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
                if (renovation.HasActiveRooms())
                {
                    Update(renovation);
                }                   
            }
        }

        private static void Update(Renovation renovation)
        {
            if (renovation.ShouldStart())
            {
                RenovationService.Start(renovation);
            }

            if (renovation.ShouldEnd())
            {
                RenovationService.End(renovation);
            }
        }

    }
}
