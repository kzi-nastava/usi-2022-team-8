using HealthInstitution.Core.RestRequestNotifications;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.RestRequests.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Core.RestRequests
{
    public class RestRequestService : IRestRequestService
    {
        IRestRequestRepository _restRequestRepository;
        IRestRequestNotificationService _restRequestNotificationService;
        IRestRequestDoctorRepository _restRequestDoctorRepository;
        public RestRequestService(IRestRequestRepository restRequestRepository, IRestRequestNotificationService restRequestNotificationService, IRestRequestDoctorRepository restRequestDoctorRepository)
        {
            _restRequestRepository = restRequestRepository;
            _restRequestNotificationService = restRequestNotificationService;
            _restRequestDoctorRepository = restRequestDoctorRepository;
        }

        public void LoadRequests()
        {
            _restRequestDoctorRepository.LoadFromFile();
        }
        private bool CheckRequestActivity(RestRequest restRequest)
        {
            return restRequest.StartDate.Date >= DateTime.Now.Date && restRequest.State == RestRequestState.OnHold;
        }
        public List<RestRequest> GetActive()
        {
            List<RestRequest> activeRestRequests = new List<RestRequest>();
            foreach (RestRequest restRequest in GetAll())
                if (CheckRequestActivity(restRequest))
                    activeRestRequests.Add(restRequest);
            return activeRestRequests;
        }
        public List<RestRequest> GetAll()
        {
            return _restRequestRepository.GetAll();
        }

        public RestRequest GetById(int id)
        {
            return _restRequestRepository.GetById(id);
        }

        public void Add(RestRequestDTO restRequestDTO)
        {
            RestRequest restRequest = new RestRequest(restRequestDTO);
            _restRequestRepository.Add(restRequest);
            if (restRequestDTO.IsUrgent)
                Accept(restRequest);
        }

        public void DeleteRequest(int id)
        {
            _restRequestRepository.Delete(id);
        }

        public void Accept(RestRequest restRequest)
        {
            _restRequestRepository.Accept(restRequest);
            _restRequestNotificationService.SendNotification(restRequest);
        }

        public void Reject(RestRequest restRequest,string rejectionReason)
        {
            _restRequestRepository.Reject(restRequest,rejectionReason);
            _restRequestNotificationService.SendNotification(restRequest);
        }

        public List<RestRequest> GetByDoctor(string doctorUsername)
        {
            return _restRequestRepository.GetByDoctor(doctorUsername);
        }
        private void Validate(RestRequestDTO restRequestDTO)
        {
            if ((restRequestDTO.StartDate - DateTime.Now).Days < 2)
            {
                throw new Exception("You have to request your days off minimum two days before the start of it!");
            }

            if (restRequestDTO.IsUrgent && !(restRequestDTO.DaysDuration > 0 && restRequestDTO.DaysDuration < 5))
                throw new Exception("Urgent requests have to be five or less days!");
            TimetableService.IsDoctorAvailable(restRequestDTO);
        }
        public void ApplyForRestRequest(RestRequestDTO restRequestDTO)
        {
            Validate(restRequestDTO);
            Add(restRequestDTO);
        }
    }
}
