using HealthInstitution.Core.RestRequestNotifications;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.RestRequests.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RestRequests
{
    public class RestRequestService : IRestRequestService
    {
        IRestRequestRepository _restRequestRepository;
        IRestRequestNotificationService _restRequestNotificationService;

        public RestRequestService(IRestRequestRepository restRequestRepository, IRestRequestNotificationService restRequestNotificationService)
        {
            _restRequestRepository = restRequestRepository;
            _restRequestNotificationService = restRequestNotificationService;
        }

        public void LoadRequests()
        {
            RestRequestDoctorRepository.GetInstance();
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
            RestRequest RestRequest = new RestRequest(restRequestDTO);
            _restRequestRepository.Add(RestRequest);
        }

        public void DeleteRequest(int id)
        {
            _restRequestRepository.Delete(id);
        }

        public void AcceptRestRequest(RestRequest restRequest)
        {
            _restRequestRepository.AcceptRestRequest(restRequest);
            _restRequestNotificationService.SendNotification(restRequest);
        }

        public void RejectRestRequest(RestRequest restRequest, string rejectionReason)
        {
            _restRequestRepository.RejectRestRequest(restRequest, rejectionReason);
            _restRequestNotificationService.SendNotification(restRequest);
        }
    }
}
