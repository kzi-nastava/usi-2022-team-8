using HealthInstitution.Core.RestRequests.Model;

namespace HealthInstitution.Core.RestRequests
{
    public interface IRestRequestService
    {
        void AcceptRestRequest(RestRequest restRequest);
        void Add(RestRequestDTO restRequestDTO);
        void DeleteRequest(int id);
        List<RestRequest> GetActive();
        List<RestRequest> GetAll();
        RestRequest GetById(int id);
        void LoadRequests();
        void RejectRestRequest(RestRequest restRequest, string rejectionReason);
    }
}