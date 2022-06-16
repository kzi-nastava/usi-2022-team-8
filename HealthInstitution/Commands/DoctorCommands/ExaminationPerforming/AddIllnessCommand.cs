using HealthInstitution.Core;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.SchedulePerforming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.ExaminationPerforming
{
    internal class AddIllnessCommand : CommandBase
    {
        private PerformExaminationDialogViewModel _performExaminationDialogViewModel;

        public AddIllnessCommand(PerformExaminationDialogViewModel performExaminationDialogViewModel)
        {
            _performExaminationDialogViewModel = performExaminationDialogViewModel;
        }

        public override void Execute(object? parameter)
        {
            String illness = _performExaminationDialogViewModel.Illness;
            if (illness != "")
            {
                _performExaminationDialogViewModel.PreviousIllnesses.Add(illness);
                _performExaminationDialogViewModel.Illness = String.Empty;
            }
        }
    }
}