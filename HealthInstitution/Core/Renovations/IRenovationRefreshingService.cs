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
    }
}
