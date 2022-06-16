using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RestRequestNotifications.Model
{
    public class RestRequestNotification
    {
        public int Id { get; set; }
        public RestRequest RestRequest { get; set; }
        public bool Active { get; set; }
        public RestRequestNotification(int id, RestRequest restRequest, bool active)
        {
            Id = id;
            RestRequest = restRequest;
            Active = active;
        }
        public RestRequestNotification(RestRequestNotificationDTO restRequestNotificationDTO)
        {
            RestRequest = restRequestNotificationDTO.RestRequest;
            Active = restRequestNotificationDTO.Active;
        }
}
}
