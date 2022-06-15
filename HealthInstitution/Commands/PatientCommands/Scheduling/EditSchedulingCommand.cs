using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.GUI.PatientWindows;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.Scheduling
{
    public class EditSchedulingCommand : CommandBase
    {
        private PatientScheduleWindowViewModel _patientScheduleWindowViewModel;

        public EditSchedulingCommand(PatientScheduleWindowViewModel patientScheduleWIndowViewModel)
        {
            _patientScheduleWindowViewModel = patientScheduleWIndowViewModel;
        }

        public override void Execute(object? parameter)
        {
            TrollCounterService.TrollCheck(_patientScheduleWindowViewModel.LoggedPatient.Username);
            Examination selectedExamination = _patientScheduleWindowViewModel.GetSelectedExamination();
            new EditExaminationDialog(selectedExamination)
            {
                DataContext = new EditExaminationDialogViewModel(_patientScheduleWindowViewModel.Examinations
                [_patientScheduleWindowViewModel.SelectedExaminationIndex])
            }.ShowDialog();
            _patientScheduleWindowViewModel.RefreshGrid();
            TrollCounterService.AppendEditDeleteDates(_patientScheduleWindowViewModel.LoggedPatient.Username);
        }

        public override bool CanExecute(object? parameter)
        {
            return _patientScheduleWindowViewModel.SelectedExaminationIndex >= 0 && base.CanExecute(parameter);
        }
    }
}