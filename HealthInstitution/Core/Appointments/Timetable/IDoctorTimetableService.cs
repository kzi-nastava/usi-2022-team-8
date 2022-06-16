using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Timetable
{
    public interface IDoctorTimetableService
    {
        public List<Examination> GetScheduledExaminations(Doctor doctor);

        public List<Operation> GetScheduledOperations(Doctor doctor);

        public List<Examination> GetExaminationsInThreeDays(List<Examination> examinations);

        public List<Operation> GetOperationsInThreeDays(List<Operation> operations);

        public List<Examination> GetExaminationsByDate(List<Examination> examinations, DateTime date);

        public List<Operation> GetOperationsByDate(List<Operation> operations, DateTime date);
    }
}