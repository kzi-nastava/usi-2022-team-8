using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core
{
    public class TimetableService 
    {
        public static List<Examination> GetScheduledExaminations(Doctor doctor)
        {
            var scheduledExaminations = new List<Examination>();
            foreach (var examination in doctor.Examinations)
            {
                if (examination.Status == ExaminationStatus.Scheduled)
                    scheduledExaminations.Add(examination);
            }
            return scheduledExaminations;
        }

        public static List<Operation> GetScheduledOperations(Doctor doctor)
        {
            var scheduledOperations = new List<Operation>();
            foreach (var operation in doctor.Operations)
            {
                if (operation.Status == ExaminationStatus.Scheduled)
                    scheduledOperations.Add(operation);
            }
            return scheduledOperations;
        }

        public static List<Examination> GetExaminationsInThreeDays(List<Examination> examinations)
        {
            var upcomingExaminations = new List<Examination>();
            DateTime today = DateTime.Now;
            DateTime dateForThreeDays = today.AddDays(3);
            foreach (Examination examination in examinations)
            {
                if (examination.Appointment <= dateForThreeDays && examination.Appointment >= today)
                    upcomingExaminations.Add(examination);
            }
            return upcomingExaminations;
        }
        public static List<Operation> GetOperationsInThreeDays(List<Operation> operations)
        {
            var upcomingOperations = new List<Operation>();
            DateTime today = DateTime.Now;
            DateTime dateForThreeDays = today.AddDays(3);
            foreach (Operation operation in upcomingOperations)
            {
                if (operation.Appointment <= dateForThreeDays && operation.Appointment >= today)
                    upcomingOperations.Add(operation);
            }
            return upcomingOperations;
        }

        public static List<Examination> GetExaminationsByDate(List<Examination> examinations, DateTime date)
        {
            var examinationsForDate = new List<Examination>();
            foreach (Examination examination in examinations)
            {
                if (examination.Appointment.Date == date)
                    examinationsForDate.Add(examination);
            }
            return examinationsForDate;
        }
        public static List<Operation> GetOperationsByDate(List<Operation> operations, DateTime date)
        {
            var operationsForDate = new List<Operation>();
            foreach (Operation operation in operations)
            {
                if (operation.Appointment.Date == date)
                    operationsForDate.Add(operation);
            }
            return operationsForDate;
        }
    }
}
