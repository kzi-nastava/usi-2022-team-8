using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.ScheduleEditRequests
{
    public interface IScheduleEditRequestsService
    {
        public List<ScheduleEditRequest> GetAll();
        public ScheduleEditRequest GetById(int id);
        public void AddEditRequest(Examination examination);
        public void AddDeleteRequest(Examination examination);
        public void DeleteRequest(int id);
        public void AcceptScheduleEditRequests(int id);
        public void RejectScheduleEditRequests(int id);
    }
}
