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
    public class ScheduleEditRequestService : IScheduleEditRequestsService
    {
        IScheduleEditRequestFileRepository _scheduleEditRequestRepository;
        IExaminationRepository _examinationRepository;
        public ScheduleEditRequestService(IScheduleEditRequestFileRepository scheduleEditRequestRepository, IExaminationRepository examinationRepository)
        {
            _scheduleEditRequestRepository = scheduleEditRequestRepository;
            _examinationRepository = examinationRepository;
        }
        public List<ScheduleEditRequest> GetAll()
        {
            return _scheduleEditRequestRepository.GetAll();
        }

        public ScheduleEditRequest GetById(int id)
        {
            return _scheduleEditRequestRepository.GetById(id);  
        }

        public void AddEditRequest(Examination examination)
        {
            int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(unixTimestamp, examination, examination.Id, _examinationRepository.GetById(examination.Id), RestRequestState.OnHold);
            _scheduleEditRequestRepository.AddEditRequest(scheduleEditRequest, unixTimestamp);
        }

        public void AddDeleteRequest(Examination examination)
        {
            Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            ScheduleEditRequest scheduleEditRequest = new ScheduleEditRequest(unixTimestamp, null, examination.Id, _examinationRepository.GetById(examination.Id), RestRequestState.OnHold);
            _scheduleEditRequestRepository.AddDeleteRequest(scheduleEditRequest, unixTimestamp);
        }

        public void DeleteRequest(int id)
        {
            ScheduleEditRequest scheduleEditRequest = GetById(id);
            if (scheduleEditRequest != null)
            {
                _scheduleEditRequestRepository.DeleteRequest(scheduleEditRequest);
            }
        }

        public void AcceptScheduleEditRequests(int id)
        {
            ScheduleEditRequest scheduleEditRequest = GetById(id);
            if (scheduleEditRequest != null)
            {
                _examinationRepository.SwapExaminationValue(scheduleEditRequest.NewExamination);
                _scheduleEditRequestRepository.AcceptScheduleEditRequests(scheduleEditRequest);
            }
        }

        public void RejectScheduleEditRequests(int id)
        {
            ScheduleEditRequest scheduleEditRequest = GetById(id);
            if (scheduleEditRequest != null)
                _scheduleEditRequestRepository.RejectScheduleEditRequests(scheduleEditRequest);
        }
    }
}
