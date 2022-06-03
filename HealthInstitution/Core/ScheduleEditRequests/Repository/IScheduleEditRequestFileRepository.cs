using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.ScheduleEditRequests.Repository
{
    public interface IScheduleEditRequestFileRepository : IRepository<ScheduleEditRequest>
    {
        public Examination ParseExamination(JToken? request, int id);
        public Examination ParseLoadedExamination(JToken? request, int id);
        public void LoadFromFile();
        public void Save();
        public dynamic MakeWithoutExamination(ScheduleEditRequest scheduleEditRequest);
        public dynamic MakeWithExamination(ScheduleEditRequest scheduleEditRequest);
        public List<dynamic> PrepareForSerialization();
        public List<ScheduleEditRequest> GetAll();
        public ScheduleEditRequest GetById(int id);
        public void AddEditRequest(ScheduleEditRequest scheduleEditRequest, int unixTimestamp);
        public void AddDeleteRequest(ScheduleEditRequest scheduleEditRequest, int unixTimestamp);
        public void DeleteRequest(ScheduleEditRequest scheduleEditRequest);
        public void AcceptScheduleEditRequests(ScheduleEditRequest scheduleEditRequest);
        public void RejectScheduleEditRequests(ScheduleEditRequest scheduleEditRequest);
    }
}
