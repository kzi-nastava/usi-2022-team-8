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
    public class RestRequestNotificationService : IRestRequestNotificationService
    {
        IRestRequestNotificationRepository _restRequestNotificationRepository;

        public RestRequestNotificationService(IRestRequestNotificationRepository restRequestNotificationRepository)
        {
            _restRequestNotificationRepository = restRequestNotificationRepository;
        }

        public void ChangeActiveStatus(RestRequestNotification notification)
        {
            notification.Active = false;
            _restRequestNotificationRepository.Save();
        }
        private void Add(RestRequestNotificationDTO restRequestNotificationDTO)
        {
            RestRequestNotification restRequestNotification = new RestRequestNotification(restRequestNotificationDTO);
            _restRequestNotificationRepository.Add(restRequestNotification);
        }
        public void SendNotification(RestRequest restRequest)
        {
            RestRequestNotificationDTO restRequestNotificationDTO = new RestRequestNotificationDTO(restRequest, true);
            Add(restRequestNotificationDTO);
        }
    }
}
