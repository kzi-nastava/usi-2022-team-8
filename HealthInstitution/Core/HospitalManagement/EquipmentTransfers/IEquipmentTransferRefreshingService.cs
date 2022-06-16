using HealthInstitution.Core.EquipmentTransfers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.EquipmentTransfers
{
    public interface IEquipmentTransferRefreshingService
    {
        public void UpdateByTransfer();
    }
}
