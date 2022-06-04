using HealthInstitution.Core.Operations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public interface IPatientOperationAvailabilityService
    {
        public void CheckIfPatientHasExaminations(OperationDTO operationDTO, int id);
        public void CheckIfPatientHasOperations(OperationDTO operationDTO, int id);
        public void CheckIfPatientIsAvailable(OperationDTO operationDTO, int id = 0);
    }
}
