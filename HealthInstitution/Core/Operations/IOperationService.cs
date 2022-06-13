using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Operations
{
    public interface IOperationService
    {
        public List<Operation> GetAll();
        public Operation GetById(int id);
        public Operation Add(OperationDTO operationDTO);
        public void Update(int id, OperationDTO operationDTO);
        public void Delete(int id);
        public List<Operation> GetByPatient(string patientUsername);
        public List<Operation> GetByDoctor(string doctorUsername);
    }
}
