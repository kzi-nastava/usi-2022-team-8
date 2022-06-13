using HealthInstitution.Core.RestRequests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RestRequestNotifications.Model
{
    public class RestRequestNotificationDTO
    {
        public RestRequest RestRequest { get; set; }
        public bool Active { get; set; }
        public RestRequestNotificationDTO(RestRequest restRequest, bool active)
        {
            RestRequest = restRequest;
            Active = active;
        }
    }
}
