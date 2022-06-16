using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.Scheduling
{
    internal class CreateOperationCommand : CommandBase
    {
        private OperationTableViewModel _operationTableViewModel;
        private Doctor _loggedDoctor;

        public CreateOperationCommand(OperationTableViewModel operationTableViewModel, Doctor doctor)
        {
            _operationTableViewModel = operationTableViewModel;
            _loggedDoctor = doctor;
        }

        public override void Execute(object? parameter)
        {
            new AddOperationDialog(_loggedDoctor).ShowDialog();
            _operationTableViewModel.RefreshGrid();
        }
    }
}