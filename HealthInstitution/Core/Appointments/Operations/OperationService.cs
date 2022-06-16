using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;

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

        public void Validate(OperationDTO operationDTO)
        {
            if (operationDTO.Appointment <= DateTime.Now)
                throw new Exception("You have to change dates for upcoming ones!");
            if (operationDTO.Duration <= 15)
                throw new Exception("Operation can't last less than 15 minutes!");
            if (operationDTO.MedicalRecord.Patient.Blocked != BlockState.NotBlocked)
                throw new Exception("Patient is blocked and can not have any examinations!");

        }

        public void Update(int id, OperationDTO operationDTO)
        {
            Validate(operationDTO);
            Operation operation = new Operation(operationDTO);
            _doctorOperationAvailabilityService.CheckIfDoctorIsAvailable(operationDTO, id);
            DIContainer.DIContainer.GetService<IPatientOperationAvailabilityService>().CheckIfPatientIsAvailable(operationDTO, id);
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
