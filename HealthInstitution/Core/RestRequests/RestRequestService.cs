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
            RestRequest restRequest = new RestRequest(restRequestDTO);
            s_restRequestRepository.Add(restRequest);
            if (restRequestDTO.IsUrgent)
                Accept(restRequest);
        }

        public static void Delete(int id)
        {
            s_restRequestRepository.Delete(id);
        }

        public static void Accept(RestRequest restRequest)
        {
            s_restRequestRepository.Accept(restRequest);
        }

        public static void Reject(RestRequest restRequest,string rejectionReason)
        {
            s_restRequestRepository.Reject(restRequest,rejectionReason);
        }

        public static List<RestRequest> GetByDoctor(string doctorUsername)
        {
            return s_restRequestRepository.GetByDoctor(doctorUsername);
        }

        private static void Validate(RestRequestDTO restRequestDTO)
        {
            if ((restRequestDTO.StartDate - DateTime.Now).Days < 2)
            {
                throw new Exception("You have to request your days off minimum two days before the start of it!");
            }

            if (restRequestDTO.IsUrgent && !(restRequestDTO.DaysDuration > 0 && restRequestDTO.DaysDuration < 5))
                throw new Exception("Urgent requests have to be five or less days!");
            TimetableService.IsDoctorAvailable(restRequestDTO);
        }
        public static void ApplyForRestRequest(RestRequestDTO restRequestDTO)
        {
            Validate(restRequestDTO);
            Add(restRequestDTO);
        }
    }
}
