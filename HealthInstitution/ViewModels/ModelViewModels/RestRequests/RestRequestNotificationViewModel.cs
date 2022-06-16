using HealthInstitution.Core.RestRequestNotifications.Model;
using HealthInstitution.Core.RestRequests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.ModelViewModels.RestRequests
{
    public class RestRequestNotificationViewModel : ViewModelBase
    {
        private RestRequestNotification _restRequestNotification;
        public DateTime StartDate => _restRequestNotification.RestRequest.StartDate;
        public int DaysDuration => _restRequestNotification.RestRequest.DaysDuration;
        public string Reason => _restRequestNotification.RestRequest.Reason;
        public string RejectionReason => _restRequestNotification.RestRequest.RejectionReason;
        public RestRequestState State => _restRequestNotification.RestRequest.State;
        public RestRequestNotificationViewModel(RestRequestNotification restRequestNotification)
        {
            _restRequestNotification = restRequestNotification;
        }
    }
}
