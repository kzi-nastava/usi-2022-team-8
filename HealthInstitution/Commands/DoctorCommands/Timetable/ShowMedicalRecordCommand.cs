using HealthInstitution.Core;
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.Timetable
{
    internal class ShowMedicalRecordCommand : CommandBase
    {
        private ScheduledExaminationTableViewModel _scheduledExaminationTableViewModel;

        public ShowMedicalRecordCommand(ScheduledExaminationTableViewModel scheduledExaminationTableViewModel)
        {
            _scheduledExaminationTableViewModel = scheduledExaminationTableViewModel;
        }

        public override void Execute(object? parameter)
        {
            new MedicalRecordDialog(_scheduledExaminationTableViewModel.GetSelectedMedicalRecord()).ShowDialog();
        }
    }
}