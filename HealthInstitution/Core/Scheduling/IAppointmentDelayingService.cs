using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public interface IAppointmentDelayingService
    {
        public DateTime FindFirstAvailableAppointment(DateTime appointment, int appointmentCounter, TimeSpan ts);

        public void GetExaminationsWithPriorities(List<Examination> nextTwoHoursExaminations, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations);

        public void GetOperationsWithPriorities(List<Operation> nextTwoHoursOperations, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations);

        public List<Tuple<int, int, DateTime>> GetPriorityExaminationsAndOperations(List<Examination> nextTwoHoursExaminations, List<Operation> nextTwoHoursOperations);

        public List<Tuple<int, int, DateTime>> FindClosest(List<DateTime> nextTwoHoursAppointments, SpecialtyType specialtyType);

        public List<DateTime> FindNextTwoHoursAppointments();
    }
}
