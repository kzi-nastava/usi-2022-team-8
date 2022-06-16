using HealthInstitution.Core.RestRequests.Model;

namespace HealthInstitution.Core.RestRequests
{
    public interface IRestRequestService
    {
        void Accept(RestRequest restRequest);
        void Add(RestRequestDTO restRequestDTO);
        void DeleteRequest(int id);
        List<RestRequest> GetActive();
        List<RestRequest> GetAll();
        RestRequest GetById(int id);
        void LoadRequests();
        void Reject(RestRequest restRequest, string rejectionReason);
        public List<RestRequest> GetByDoctor(string doctorUsername);
        public void ApplyForRestRequest(RestRequestDTO restRequestDTO);
    }
}