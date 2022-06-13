using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.ScheduleEditRequests.Model;
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
        public List<Tuple<int, int, DateTime>> FindClosest(List<DateTime> nextTwoHoursAppointments, SpecialtyType specialtyType, RoomType roomType);
        public List<ScheduleEditRequest> PrepareDataForDelaying(List<Tuple<int, int, DateTime>> examinationsAndOperationsForDelaying);
        public void DelayExamination(ScheduleEditRequest selectedAppointment, Examination examination);
        public void DelayOperation(ScheduleEditRequest selectedAppointment, Operation operation);

    }
}
