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
        IExaminationService _examinationService;
        IDoctorService _doctorService;
        public DeleteExaminationCommand(ExaminationTableViewModel examinationTableViewModel, IExaminationService examinationService, IDoctorService doctorService)
        {
            _examinationTableViewModel = examinationTableViewModel;
            _examinationService = examinationService;
            _doctorService = doctorService;
        }
        public override void Execute(object? parameter)
        {
            var answer = System.Windows.MessageBox.Show("Are you sure you want to delete selected examination?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                var selectedExamination = _examinationTableViewModel.GetSelectedExamination();
                _examinationService.Delete(selectedExamination.Id);
                _doctorService.DeleteExamination(selectedExamination);
                _examinationTableViewModel.RefreshGrid();
            }
        }

    }
}