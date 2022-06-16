using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;

namespace HealthInstitution.Core.Operations
{
    public static class OperationService
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

        public static Operation Add(OperationDTO operationDTO)
        {
            Operation operation = new Operation(operationDTO);
            s_operationRepository.Add(operation);
            return operation;
        }

        public static void Validate(OperationDTO operationDTO)
        {
            if (operationDTO.Appointment <= DateTime.Now)
                throw new Exception("You have to change dates for upcoming ones!");
            if (operationDTO.Duration <= 15)
                throw new Exception("Operation can't last less than 15 minutes!");
            if (operationDTO.MedicalRecord.Patient.Blocked != BlockState.NotBlocked)
                throw new Exception("Patient is blocked and can not have any examinations!");

        }

        public static void Update(int id, OperationDTO operationDTO)
        {
            Validate(operationDTO);
            Operation operation = new Operation(operationDTO);
            DoctorOperationAvailabilityService.CheckIfDoctorIsAvailable(operationDTO, id);
            PatientOperationAvailabilityService.CheckIfPatientIsAvailable(operationDTO, id);
            s_operationRepository.Update(id, operation);
        }

        public static void Delete(int id)
        {
            s_operationRepository.Delete(id);
        }

        public static List<Operation> GetByPatient(String patientUsername)
        {
            return s_operationRepository.GetByPatient(patientUsername);
        }

        public static List<Operation> GetByDoctor(String doctorUsername)
        {
            return s_operationRepository.GetByDoctor(doctorUsername);
        }
    }
}
