using HealthInstitution.Core;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.Timetable
{
    internal class ShowDataGridCommand : CommandBase
    {
        private ScheduledExaminationTableViewModel _scheduledExaminationTableViewModel;

        public ShowDataGridCommand(ScheduledExaminationTableViewModel scheduledExaminationTableViewModel)
        {
            _scheduledExaminationTableViewModel = scheduledExaminationTableViewModel;
        }

        public override void Execute(object? parameter)
        {
            _scheduledExaminationTableViewModel.RefreshGrid();
        }
    }
}