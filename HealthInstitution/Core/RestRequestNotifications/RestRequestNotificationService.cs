using HealthInstitution.Core.RestRequestNotifications.Model;
using HealthInstitution.Core.RestRequestNotifications.Repository;
using HealthInstitution.Core.RestRequests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RestRequestNotifications
{
    public static class RestRequestNotificationService
    {
        private static RestRequestNotificationRepository s_restRequestNotificationRepository = RestRequestNotificationRepository.GetInstance();
        public static void ChangeActiveStatus(RestRequestNotification notification)
        {
            notification.Active = false;
            s_restRequestNotificationRepository.Save();
        }
        private static void Add(RestRequestNotificationDTO restRequestNotificationDTO)
        {
            RestRequestNotification restRequestNotification = new RestRequestNotification(restRequestNotificationDTO);
            s_restRequestNotificationRepository.Add(restRequestNotification);
        }
        public static void SendNotification(RestRequest restRequest)
        {
            RestRequestNotificationDTO restRequestNotificationDTO = new RestRequestNotificationDTO(restRequest, true);
            Add(restRequestNotificationDTO);
        }
    }
}
