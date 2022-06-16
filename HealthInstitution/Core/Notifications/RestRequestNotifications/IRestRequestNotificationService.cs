using HealthInstitution.Core.RestRequestNotifications.Model;
using HealthInstitution.Core.RestRequests.Model;

namespace HealthInstitution.Core.RestRequestNotifications
{
    public interface IRestRequestNotificationService
    {
        void ChangeActiveStatus(RestRequestNotification notification);
        void SendNotification(RestRequest restRequest);
    }
}