using HealthInstitution.Core;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.Scheduling
{
    internal class DeleteOperationCommand : CommandBase
    {
        private OperationTableViewModel _operationTableViewModel;
        public DeleteOperationCommand(OperationTableViewModel operationTableViewModel)
        {
            _operationTableViewModel = operationTableViewModel;
        }
        public override void Execute(object? parameter)
        {
            var answer = System.Windows.MessageBox.Show("Are you sure you want to delete selected operation?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                var selectedOperation = _operationTableViewModel.GetSelectedOperation();
                OperationService.Delete(selectedOperation.Id);
                DoctorService.DeleteOperation(selectedOperation);
                _operationTableViewModel.RefreshGrid();
            }
        }

    }
}
