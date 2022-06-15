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
    private PatientScheduleWindowViewModel _patientScheduleWindowViewModel;

    public AddSchedulingCommand(PatientScheduleWindowViewModel patientScheduleWindowViewModel)
    {
        _patientScheduleWindowViewModel = patientScheduleWindowViewModel;
    }

    public override void Execute(object? parameter)
    {
        try
        {
            TrollCounterService.TrollCheck(_patientScheduleWindowViewModel.LoggedPatient.Username);
            new AddExaminationDialog(_patientScheduleWindowViewModel.LoggedPatient)
            {
                DataContext = new AddExaminationDialogViewModel(_patientScheduleWindowViewModel.LoggedPatient)
            }.ShowDialog();
            _patientScheduleWindowViewModel.RefreshGrid();
            TrollCounterService.AppendCreateDates(_patientScheduleWindowViewModel.LoggedPatient.Username);
            MessageBox.Show("Sucessfuly made examination appointment", "Success");
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error");
        }
    }
}