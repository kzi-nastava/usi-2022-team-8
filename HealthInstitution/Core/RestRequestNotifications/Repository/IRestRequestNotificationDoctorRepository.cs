namespace HealthInstitution.Core.RestRequestNotifications.Repository
{
    public interface IRestRequestNotificationDoctorRepository
    {
        void LoadFromFile();
        void Save();
    }
}