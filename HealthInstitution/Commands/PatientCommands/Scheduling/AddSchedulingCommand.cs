using HealthInstitution.Core;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.Scheduling;

public class AddSchedulingCommand : CommandBase
{
    private PatientScheduleWindowViewModel _patientScheduleWIndowViewModel;

    public AddSchedulingCommand(PatientScheduleWindowViewModel patientScheduleWIndowViewModel)
    {
        _patientScheduleWIndowViewModel = patientScheduleWIndowViewModel;
    }

    public override void Execute(object? parameter)
    {
        TrollCounterService.TrollCheck(_patientScheduleWIndowViewModel.LoggedPatient.Username);
        new AddExaminationDialog(_patientScheduleWIndowViewModel.LoggedPatient).ShowDialog();
        _patientScheduleWIndowViewModel.RefreshGrid();
        TrollCounterService.AppendCreateDates(_patientScheduleWIndowViewModel.LoggedPatient.Username);
    }
}