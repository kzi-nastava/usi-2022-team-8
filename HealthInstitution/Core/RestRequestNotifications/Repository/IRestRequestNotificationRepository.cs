using HealthInstitution.Core.RestRequestNotifications.Model;

namespace HealthInstitution.Core.RestRequestNotifications.Repository
{
    public interface IRestRequestNotificationRepository
    {
        void Add(RestRequestNotification restRequestNotification);
        List<RestRequestNotification> GetAll();
        RestRequestNotification GetById(int id);
        void LoadFromFile();
        void Save();
    }
}