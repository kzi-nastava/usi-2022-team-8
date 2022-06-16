using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
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
        ITrollCounterService _trollCounterService;

        public EditSchedulingCommand(PatientScheduleWindowViewModel patientScheduleWIndowViewModel, ITrollCounterService trollCounterService)
        {
            _patientScheduleWindowViewModel = patientScheduleWIndowViewModel;
            _trollCounterService = trollCounterService;
        }

        public override void Execute(object? parameter)
        {
            _trollCounterService.TrollCheck(_patientScheduleWindowViewModel.LoggedPatient.Username);
            Examination selectedExamination = _patientScheduleWindowViewModel.GetSelectedExamination();
            var window = DIContainer.GetService<EditExaminationDialog>();
            window.SetExamination(selectedExamination);
            window.ShowDialog();
            _patientScheduleWindowViewModel.RefreshGrid();
            _trollCounterService.AppendEditDeleteDates(_patientScheduleWindowViewModel.LoggedPatient.Username);
        }

        public override bool CanExecute(object? parameter)
        {
            return _patientScheduleWindowViewModel.SelectedExaminationIndex >= 0 && base.CanExecute(parameter);
        }
    }
}