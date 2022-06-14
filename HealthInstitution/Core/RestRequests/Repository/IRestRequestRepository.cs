using HealthInstitution.Core.RestRequests.Model;

namespace HealthInstitution.Core.RestRequests.Repository
{
    public interface IRestRequestRepository
    {
        void AcceptRestRequest(RestRequest restRequest);
        void Add(RestRequest RestRequest);
        void Delete(int id);
        List<RestRequest> GetAll();
        RestRequest GetById(int id);
        void LoadFromFile();
        void RejectRestRequest(RestRequest restRequest, string rejectionReason);
        void Save();
        void Update(int id, RestRequest byRestRequest);
    }
}