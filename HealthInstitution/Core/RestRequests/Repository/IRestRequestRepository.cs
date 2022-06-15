using HealthInstitution.Core.RestRequests.Model;

namespace HealthInstitution.Core.RestRequests.Repository
{
    public interface IRestRequestRepository
    {
        void Accept(RestRequest restRequest);
        void Add(RestRequest RestRequest);
        void Delete(int id);
        List<RestRequest> GetAll();
        Dictionary<int, RestRequest> GetAllById();
        RestRequest GetById(int id);
        void LoadFromFile();
        void Reject(RestRequest restRequest, string rejectionReason);
        void Save();
        void Update(int id, RestRequest byRestRequest);
        public List<RestRequest> GetByDoctor(string doctorUsername);
    }
}