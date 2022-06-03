using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Patients.Model;

namespace HealthInstitution.Core.Operations
{
    internal static class OperationService
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

        public static void Update(int id, OperationDTO operationDTO)
        {
            Operation operation = new Operation(operationDTO);
            DoctorOperationAvailabilityService.CheckIfDoctorIsAvailable(operationDTO, id);
            PatientOperationAvailabilityService.CheckIfPatientIsAvailable(operationDTO, id);
            s_operationRepository.Update(id, operation);
        }

        public static void Delete(int id)
        {
            s_operationRepository.Delete(id);
        }

        public static List<Operation> GetByPatient(String username)
        {
            return s_operationRepository.GetByPatient(username);
        }
    }
}
