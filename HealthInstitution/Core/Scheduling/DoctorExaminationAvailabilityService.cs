using HealthInstitution.Core.Examinations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public static class DoctorExaminationAvailabilityService
    {
        private static void CheckIfDoctorHasOperations(ExaminationDTO examinationDTO)
        {
            var doctor = examinationDTO.Doctor;
            DateTime appointment = examinationDTO.Appointment;

            foreach (var operation in doctor.Operations)
            {
                if ((appointment < operation.Appointment.AddMinutes(operation.Duration)) && (appointment.AddMinutes(15) > operation.Appointment))
                {
                    throw new Exception("That doctor is not available");
                }
            }
        }

        private static void CheckIfDoctorHasExaminations(ExaminationDTO examinationDTO)
        {
            var doctor = examinationDTO.Doctor;
            DateTime appointment = examinationDTO.Appointment;

            foreach (var examination in doctor.Examinations)
            {
                if (examination.Appointment == appointment)
                {
                    throw new Exception("That doctor is not available");
                }
            }
        }

        public static void CheckIfDoctorIsAvailable(ExaminationDTO examinationDTO)
        {
            CheckIfDoctorHasExaminations(examinationDTO);
            CheckIfDoctorHasOperations(examinationDTO);
        }
    }
}
