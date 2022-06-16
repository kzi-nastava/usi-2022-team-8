using HealthInstitution.Core;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.RestRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.RestRequests
{
    internal class AddRestRequestCommand : CommandBase
    {
        private Doctor _loggedDoctor;
        private RestRequestTableViewModel _restRequestTableViewModel;

        public AddRestRequestCommand(RestRequestTableViewModel restRequestTableViewModel, Doctor doctor)
        {
            _restRequestTableViewModel = restRequestTableViewModel;
            _loggedDoctor = doctor;
        }

        public override void Execute(object? parameter)
        {
            new AddRestRequestDialog(_loggedDoctor).ShowDialog();
            _restRequestTableViewModel.RefreshGrid();
        }
    }
}