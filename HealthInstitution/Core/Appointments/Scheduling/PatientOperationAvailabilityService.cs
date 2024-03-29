﻿using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public class PatientOperationAvailabilityService : IPatientOperationAvailabilityService
    {
        IExaminationService _examinationService;
        IOperationService _operationService;

        public PatientOperationAvailabilityService(IExaminationService examinationService, IOperationService operationService)
        {
            _examinationService = examinationService;
            _operationService = operationService;
        }

        private void CheckIfPatientHasExaminations(OperationDTO operationDTO, int id)
        {
            Patient patient = operationDTO.MedicalRecord.Patient;
            DateTime appointment = operationDTO.Appointment;
            int duration = operationDTO.Duration;
            var patientExaminations = _examinationService.GetByPatient(patient.Username);

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
            var patientOperations = _operationService.GetByPatient(patient.Username);

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

        public void CheckIfPatientIsAvailable(OperationDTO operationDTO, int id = 0)
        {
            CheckIfPatientHasExaminations(operationDTO, id);
            CheckIfPatientHasOperations(operationDTO, id);
        }
    }
}
