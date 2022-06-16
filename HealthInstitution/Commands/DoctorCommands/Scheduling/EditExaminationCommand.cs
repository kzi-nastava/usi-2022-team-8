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
    internal class EditExaminationCommand : CommandBase
    {
        private ExaminationTableViewModel _examinationTableViewModel;

        public EditExaminationCommand(ExaminationTableViewModel examinationTableViewModel)
        {
            _examinationTableViewModel = examinationTableViewModel;
        }

        public override void Execute(object? parameter)
        {
            new EditExaminationDialog(_examinationTableViewModel.GetSelectedExamination()).ShowDialog();
            _examinationTableViewModel.RefreshGrid();
        }
    }
}