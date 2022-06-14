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
    public class RestRequestService
    {
        static RestRequestRepository s_restRequestRepository = RestRequestRepository.GetInstance();

        public static void LoadRequests()
        {
            RestRequestDoctorRepository.GetInstance();
        }
        private static bool CheckRequestActivity(RestRequest restRequest)
        {
            return restRequest.StartDate.Date >= DateTime.Now.Date && restRequest.State == RestRequestState.OnHold;
        }
        public static List<RestRequest> GetActive()
        {
            List<RestRequest> activeRestRequests = new List<RestRequest>();
            foreach(RestRequest restRequest in GetAll())
                if(CheckRequestActivity(restRequest))
                    activeRestRequests.Add(restRequest);
            return activeRestRequests;
        }
        public static List<RestRequest> GetAll()
        {
            return s_restRequestRepository.GetAll();
        }

        public static RestRequest GetById(int id)
        {
            return s_restRequestRepository.GetById(id);
        }

        public static void Add(RestRequestDTO restRequestDTO)
        {
            RestRequest RestRequest = new RestRequest(restRequestDTO);
            s_restRequestRepository.Add(RestRequest);
        }

        public static void DeleteRequest(int id)
        {
            s_restRequestRepository.Delete(id);
        }

        public static void AcceptRestRequest(RestRequest restRequest)
        {
            s_restRequestRepository.AcceptRestRequest(restRequest);
            RestRequestNotificationService.SendNotification(restRequest);
        }

        public static void RejectRestRequest(RestRequest restRequest,string rejectionReason)
        {
            s_restRequestRepository.RejectRestRequest(restRequest,rejectionReason);
            RestRequestNotificationService.SendNotification(restRequest);
        }
    }
}
