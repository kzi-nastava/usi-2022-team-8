using HealthInstitution.Core.Operations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Patients
{
    public interface IPatientOperationAvailabilityService
    {
        public void CheckIfPatientIsAvailable(OperationDTO operationDTO, int id = 0);
    }
}
