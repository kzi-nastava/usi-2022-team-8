using HealthInstitution.Core.Operations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public interface IDoctorOperationAvailabilityService
    {
        public void CheckIfDoctorIsAvailable(OperationDTO operationDTO, int id = 0);
    }
}
