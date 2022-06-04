﻿using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Patients
{
    class PatientOperationAvailabilityService
    {
        private void CheckIfPatientHasExaminations(OperationDTO operationDTO, int id)
        {
            Patient patient = operationDTO.MedicalRecord.Patient;
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;
            var patientExaminations = ExaminationRepository.GetInstance().GetPatientExaminations(patient);

            foreach (var examination in patientExaminations)
            {
                if (examination.Id == id)
                    continue;
                if ((appointment < examination.Appointment.AddMinutes(15)) && (appointment.AddMinutes(duration) > examination.Appointment))
                {
                    throw new Exception("That patient is not available");
                }
            }
        }

        private void CheckIfPatientHasOperations(OperationDTO operationDTO, int id)
        {
            Patient patient = operationDTO.MedicalRecord.Patient;
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;
            var patientOperations = OperationRepository.GetInstance().GetPatientOperations(patient);

            foreach (var operation in patientOperations)
            {
                if (operation.Id == id)
                    continue;
                if ((appointment < operation.Appointment.AddMinutes(operation.Duration)) && (appointment.AddMinutes(duration) > operation.Appointment))
                {
                    throw new Exception("That patient is not available");
                }
            }
        }

        private void CheckIfPatientIsAvailable(OperationDTO operationDTO, int id = 0)
        {
            CheckIfPatientHasExaminations(operationDTO, id);
            CheckIfPatientHasOperations(operationDTO, id);
        }
    }
}
