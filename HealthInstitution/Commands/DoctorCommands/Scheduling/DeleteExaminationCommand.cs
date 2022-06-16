using HealthInstitution.Core;
using HealthInstitution.Core.Examinations;
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
    internal class DeleteExaminationCommand : CommandBase
    {
        private ExaminationTableViewModel _examinationTableViewModel;
        public DeleteExaminationCommand(ExaminationTableViewModel examinationTableViewModel)
        {
            _examinationTableViewModel = examinationTableViewModel;
        }
        public override void Execute(object? parameter)
        {
            var answer = System.Windows.MessageBox.Show("Are you sure you want to delete selected examination?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                var selectedExamination = _examinationTableViewModel.GetSelectedExamination();
                ExaminationService.Delete(selectedExamination.Id);
                DoctorService.DeleteExamination(selectedExamination);
                _examinationTableViewModel.RefreshGrid();
            }
        }

    }
}