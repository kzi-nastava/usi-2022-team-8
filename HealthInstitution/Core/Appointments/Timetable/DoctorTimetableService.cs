using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Timetable
{
    public class DoctorTimetableService : IDoctorTimetableService
    {
        public DoctorTimetableService()
        { }

        public List<Examination> GetScheduledExaminations(Doctor doctor)
        {
            return doctor.Examinations.Where(e => e.Status == ExaminationStatus.Scheduled).ToList();
        }

        public List<Operation> GetScheduledOperations(Doctor doctor)
        {
            return doctor.Operations.Where(o => o.Status == ExaminationStatus.Scheduled).ToList();
        }

        public List<Examination> GetExaminationsInThreeDays(List<Examination> examinations)
        {
            DateTime today = DateTime.Now;
            DateTime dateForThreeDays = today.AddDays(3);
            return examinations.Where(e => e.Appointment <= dateForThreeDays && e.Appointment >= today).ToList();
        }

        public List<Operation> GetOperationsInThreeDays(List<Operation> operations)
        {
            DateTime today = DateTime.Now;
            DateTime dateForThreeDays = today.AddDays(3);
            return operations.Where(o => o.Appointment <= dateForThreeDays && o.Appointment >= today).ToList();
        }

        public List<Examination> GetExaminationsByDate(List<Examination> examinations, DateTime date)
        {
            return examinations.Where(e => e.Appointment.Date == date).ToList();
        }

        public List<Operation> GetOperationsByDate(List<Operation> operations, DateTime date)
        {
            return operations.Where(o => o.Appointment.Date == date).ToList();
        }

        public static void IsDoctorAvailable(RestRequestDTO restRequestDTO)
        {
            var examinations = restRequestDTO.Doctor.Examinations;
            var operations = restRequestDTO.Doctor.Operations;
            int numberOfDays = restRequestDTO.DaysDuration;
            DateTime startDate = restRequestDTO.StartDate;
            bool hasScheduledExaminations = examinations.Any(e => e.Appointment >= startDate && e.Appointment <= startDate.AddDays(numberOfDays));
            bool hasScheduledOperations = operations.Any(o => o.Appointment >= startDate && o.Appointment <= startDate.AddDays(numberOfDays));
            if (hasScheduledExaminations || hasScheduledOperations)
                throw new Exception("You have appointments in wanted days!");
        }
    }
}