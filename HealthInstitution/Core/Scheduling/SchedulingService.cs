using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    static class SchedulingService
    {
        static OperationRepository s_operationRepository = OperationRepository.GetInstance();
        public static List<Operation> GetAll()
        {
            return s_operationRepository.GetAll();
        }
        public static Operation GetById(int id)
        {
            return s_operationRepository.GetById(id);
        }

        public static void Add(OperationDTO operationDTO)
        {
            Operation operation = new Operation(operationDTO);
            s_operationRepository.Add(operation);
        }

        public static void Delete(int id)
        {
            s_operationRepository.Delete(id);
        }
    }
}
