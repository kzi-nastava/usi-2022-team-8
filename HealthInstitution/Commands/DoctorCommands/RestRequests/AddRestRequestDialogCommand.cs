using HealthInstitution.Core;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.RestRequests;
using HealthInstitution.GUI.DoctorView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.RestRequests
{
    internal class AddRestRequestDialogCommand : CommandBase
    {
      
        private AddRestRequestDialogViewModel _addRestRequestViewModel;
        private Doctor _loggedDoctor;
        IRestRequestService _restRequestService;

        public AddRestRequestDialogCommand(AddRestRequestDialogViewModel addRestRequestViewModel, Doctor doctor, IRestRequestService restRequestService)
        {
            _addRestRequestViewModel = addRestRequestViewModel;
            _loggedDoctor = doctor;
            _restRequestService = restRequestService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                var restRequestDTO = CreateRestRequestDTOFromInputData();
                _restRequestService.ApplyForRestRequest(restRequestDTO);
                System.Windows.MessageBox.Show("You have applied for rest days!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                _addRestRequestViewModel.ThisWindow.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public RestRequestDTO CreateRestRequestDTOFromInputData()
        {
            DateTime startDate = _addRestRequestViewModel.GetStartDate();
            int daysDuration = _addRestRequestViewModel.GetDaysDuration();
            String requestReason = _addRestRequestViewModel.GetReason();
            bool isUrgent = _addRestRequestViewModel.GetUrgencyChoice();
            RestRequestDTO restRequestDTO = new RestRequestDTO(_loggedDoctor, requestReason, startDate, daysDuration, RestRequestState.OnHold, isUrgent, "");
            return restRequestDTO;
        }
    }
}