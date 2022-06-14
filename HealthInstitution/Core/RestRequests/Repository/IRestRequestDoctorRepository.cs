namespace HealthInstitution.Core.RestRequests.Repository
{
    public interface IRestRequestDoctorRepository
    {
        void LoadFromFile();
        void Save();
    }
}