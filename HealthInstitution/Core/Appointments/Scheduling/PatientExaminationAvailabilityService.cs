﻿using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public class PatientExaminationAvailabilityService : IPatientExaminationAvailabilityService
    {
        IExaminationService _examinationService;
        IOperationService _operationService;

        public PatientExaminationAvailabilityService(IExaminationService examinationService, IOperationService operationService)
        {
            _examinationService = examinationService;
            _operationService = operationService;
        }

        private void CheckIfPatientHasExaminations(ExaminationDTO examinationDTO)
        {
            var patient = examinationDTO.MedicalRecord.Patient;
            DateTime appointment = examinationDTO.Appointment;
            var patientExaminations = _examinationService.GetByPatient(patient.Username);

            foreach (var examination in patientExaminations)
            {
                if (examination.Appointment == appointment)
                {
                    throw new Exception("That patient is not available");
                }
            }
        }

        private void CheckIfPatientHasOperations(ExaminationDTO examinationDTO)
        {
            var patient = examinationDTO.MedicalRecord.Patient;
            DateTime appointment = examinationDTO.Appointment;
            var patientOperations = _operationService.GetByPatient(patient.Username);

            foreach (var operation in patientOperations)
            {
                if ((appointment < operation.Appointment.AddMinutes(operation.Duration)) && (appointment.AddMinutes(15) > operation.Appointment))
                {
                    throw new Exception("That patient is not available");
                }
            }
        }

        public void CheckIfPatientIsAvailable(ExaminationDTO examinationDTO)
        {
            CheckIfPatientHasExaminations(examinationDTO);
            CheckIfPatientHasOperations(examinationDTO);
        }
    }
}
