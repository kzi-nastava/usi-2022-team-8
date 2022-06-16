using HealthInstitution.Core;
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.Scheduling
{
    internal class EditOperationCommand : CommandBase
    {
        private OperationTableViewModel _operationTableViewModel;
        public EditOperationCommand(OperationTableViewModel operationTableViewModel)
        {
            _operationTableViewModel = operationTableViewModel;
        }

        public override void Execute(object? parameter)
        {
            new EditOperationDialog(_operationTableViewModel.GetSelectedOperation()).ShowDialog();
            _operationTableViewModel.RefreshGrid();
        }
    }
}