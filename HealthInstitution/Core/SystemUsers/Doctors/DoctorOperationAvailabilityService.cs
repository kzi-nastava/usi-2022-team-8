using HealthInstitution.Core.Operations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Doctors
{
    class DoctorOperationAvailabilityService
    {
        private void CheckIfDoctorHasExaminations(OperationDTO operationDTO, int id = 0)
        {
            var doctor = operationDTO.Doctor;
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;

            foreach (var examination in doctor.Examinations)
            {
                if (examination.Id == id)
                    continue;
                if ((appointment < examination.Appointment.AddMinutes(15)) && (appointment.AddMinutes(duration) > examination.Appointment))
                {
                    throw new Exception("That doctor is not available");
                }
            }
        }

        private void CheckIfDoctorHasOperations(OperationDTO operationDTO, int id = 0)
        {
            var doctor = operationDTO.Doctor;
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;

            foreach (var operation in doctor.Operations)
            {
                if (operation.Id == id)
                    continue;
                if ((appointment < operation.Appointment.AddMinutes(operation.Duration)) && (appointment.AddMinutes(duration) > operation.Appointment))
                {
                    throw new Exception("That doctor is not available");
                }
            }
        }
        public void CheckIfDoctorIsAvailable(OperationDTO operationDTO, int id = 0)
        {
            CheckIfDoctorHasExaminations(operationDTO, id);
            CheckIfDoctorHasOperations(operationDTO, id);
        }
    }
}
