using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Patients.Model;

namespace HealthInstitution.Core.Operations
{
    public class OperationService : IOperationService
    {
        IOperationRepository _operationRepository;
        IDoctorOperationAvailabilityService _doctorOperationAvailabilityService;
        public OperationService(IOperationRepository operationRepository,
            IDoctorOperationAvailabilityService doctorOperationAvailabilityService)
        {
            _operationRepository = operationRepository;
            _doctorOperationAvailabilityService = doctorOperationAvailabilityService;
        }
        public int GetMaxId()
        {
            return _operationRepository.GetMaxId();
        }
        public List<Operation> GetAll()
        {
            return _operationRepository.GetAll();
        }
        public Operation GetById(int id)
        {
            return _operationRepository.GetById(id);
        }

        public Operation Add(OperationDTO operationDTO)
        {
            Operation operation = new Operation(operationDTO);
            _operationRepository.Add(operation);
            return operation;
        }

        public void Update(int id, OperationDTO operationDTO)
        {
            operationDTO.Validate();
            Operation operation = new Operation(operationDTO);
            _doctorOperationAvailabilityService.CheckIfDoctorIsAvailable(operationDTO, id);
            DIContainer.DIContainer.GetService<PatientOperationAvailabilityService>().CheckIfPatientIsAvailable(operationDTO, id);
            _operationRepository.Update(id, operation);
        }

        public void Delete(int id)
        {
            _operationRepository.Delete(id);
        }

        public List<Operation> GetByPatient(String patientUsername)
        {
            return _operationRepository.GetByPatient(patientUsername);
        }

        public List<Operation> GetByDoctor(String doctorUsername)
        {
            return _operationRepository.GetByDoctor(doctorUsername);
        }
    }
}
