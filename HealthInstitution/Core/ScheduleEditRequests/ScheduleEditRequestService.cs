using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.ScheduleEditRequests.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.ScheduleEditRequests
{
    internal static class ScheduleEditRequestService
    {
        static ScheduleEditRequestFileRepository s_scheduleEditRequestRepository = ScheduleEditRequestFileRepository.GetInstance();

        public static List<ScheduleEditRequest> GetAll()
        {
            return s_scheduleEditRequestRepository.GetAll();
        }

        public static ScheduleEditRequest GetById(int id)
        {
            return s_scheduleEditRequestRepository.GetById(id);  
        }

        public static void AddEditRequest(Examination examination)
        {
            int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(unixTimestamp, examination, examination.Id, RestRequestState.OnHold);
            s_scheduleEditRequestRepository.AddEditRequest(scheduleEditRequest, unixTimestamp);
        }

        public static void AddDeleteRequest(Examination examination)
        {
            Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(unixTimestamp, null, examination.Id, RestRequestState.OnHold);
            s_scheduleEditRequestRepository.AddDeleteRequest(scheduleEditRequest, unixTimestamp);
        }

        public static void DeleteRequest(int id)
        {
            ScheduleEditRequest scheduleEditRequest = GetById(id);
            if (scheduleEditRequest != null)
            {
                s_scheduleEditRequestRepository.DeleteRequest(scheduleEditRequest);
            }
        }

        public static void AcceptScheduleEditRequests(int id)
        {
            ScheduleEditRequest scheduleEditRequest = GetById(id);
            if (scheduleEditRequest != null)
            {
                ExaminationRepository.GetInstance().SwapExaminationValue(scheduleEditRequest.NewExamination);
                s_scheduleEditRequestRepository.AcceptScheduleEditRequests(scheduleEditRequest);
            }
        }

        public static void RejectScheduleEditRequests(int id)
        {
            ScheduleEditRequest scheduleEditRequest = GetById(id);
            if (scheduleEditRequest != null)
                s_scheduleEditRequestRepository.RejectScheduleEditRequests(scheduleEditRequest);
        }
    }
}
