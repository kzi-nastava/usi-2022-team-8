using HealthInstitution.Core;
using HealthInstitution.Core.RestRequests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.ModelViewModels.RestRequests
{
    public class RestRequestViewModel : ViewModelBase
    {
        private RestRequest _restRequest;
        public DateTime StartDate => _restRequest.StartDate;
        public int DaysDuration => _restRequest.DaysDuration;
        public string Reason => _restRequest.Reason;
        public RestRequestState State => _restRequest.State;
        public RestRequestViewModel(RestRequest restRequest)
        {
            _restRequest = restRequest;
        }
    }
}