using HealthInstitution.Commands.DoctorCommands.RestRequests;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.ViewModels.ModelViewModels.RestRequests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.RestRequests
{
    internal class RestRequestTableViewModel : ViewModelBase
    {
        public Doctor LoggedDoctor;

        public List<RestRequest> RestRequests;

        private int _selectedRestRequestIndex;

        public int SelectedRestRequestIndex
        {
            get
            {
                return _selectedRestRequestIndex;
            }
            set
            {
                _selectedRestRequestIndex = value;
                OnPropertyChanged(nameof(SelectedRestRequestIndex));
            }
        }

        private ObservableCollection<RestRequestViewModel> _restRequestsVM;

        public ObservableCollection<RestRequestViewModel> RestRequestsVM
        {
            get
            {
                return _restRequestsVM;
            }
            set
            {
                _restRequestsVM = value;
                OnPropertyChanged(nameof(RestRequestsVM));
            }
        }

        public void RefreshGrid()
        {
            _restRequestsVM.Clear();
            RestRequests.Clear();
            List<RestRequest> activeRestRequests = _restRequestService.GetByDoctor(LoggedDoctor.Username);
            foreach (RestRequest restRequest in activeRestRequests)
            {
                RestRequests.Add(restRequest);
                _restRequestsVM.Add(new RestRequestViewModel(restRequest));
            }
        }

        public RestRequest GetSelectedRestRequest()
        {
            return RestRequests[_selectedRestRequestIndex];
        }

        public ICommand CreateRequestCommand { get; }
        IRestRequestService _restRequestService;
        public RestRequestTableViewModel(Doctor loggedDoctor, IRestRequestService restRequestService)
        {
            LoggedDoctor = loggedDoctor;
            RestRequests = new();
            _restRequestsVM = new();            
            _restRequestService = restRequestService;
            RefreshGrid();
            CreateRequestCommand = new AddRestRequestCommand(this, loggedDoctor);
        }
    }
}
