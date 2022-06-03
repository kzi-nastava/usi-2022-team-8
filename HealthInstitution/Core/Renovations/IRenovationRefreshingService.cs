using HealthInstitution.Core.Renovations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Renovations
{
    public interface IRenovationRefreshingService
    {
        public void UpdateByRenovation();
        public void UpdateSeparationRenovation(RoomSeparation roomSeparation);
        public void UpdateMergeRenovation(RoomMerger roomMerger);
        public void UpdateSimpleRenovation(Renovation renovation);
    }
}
