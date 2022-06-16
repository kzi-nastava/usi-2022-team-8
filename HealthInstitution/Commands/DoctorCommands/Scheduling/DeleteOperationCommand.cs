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
        IOperationService _operationService;
        IDoctorService _doctorService;
        public DeleteOperationCommand(OperationTableViewModel operationTableViewModel, IOperationService operationService, IDoctorService doctorService)
        {
            _operationTableViewModel = operationTableViewModel;
            _operationService = operationService;
            _doctorService = doctorService;
        }
        public override void Execute(object? parameter)
        {
            var answer = System.Windows.MessageBox.Show("Are you sure you want to delete selected operation?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                var selectedOperation = _operationTableViewModel.GetSelectedOperation();
                _operationService.Delete(selectedOperation.Id);
                _doctorService.DeleteOperation(selectedOperation);
                _operationTableViewModel.RefreshGrid();
            }
        }

    }
}
