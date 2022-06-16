using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.SystemUsers.Patients.Model;
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
    Patient _loggedPatient;
    private PatientScheduleWindowViewModel _patientScheduleWindowViewModel;
    ITrollCounterService _trollCounterService;
    public AddSchedulingCommand(PatientScheduleWindowViewModel patientScheduleWindowViewModel,Patient loggedPatient, ITrollCounterService trollCounterService)
    {
        _loggedPatient = loggedPatient;
        _patientScheduleWindowViewModel = patientScheduleWindowViewModel;
        _trollCounterService = trollCounterService;
    }

    public override void Execute(object? parameter)
    {
        try
        {
            _trollCounterService.TrollCheck(_patientScheduleWindowViewModel.LoggedPatient.Username);
            var window = DIContainer.GetService<AddExaminationDialog>();
            window.SetLoggedPatient(_loggedPatient);
            window.ShowDialog();
            _patientScheduleWindowViewModel.RefreshGrid();
            _trollCounterService.AppendCreateDates(_patientScheduleWindowViewModel.LoggedPatient.Username);
            MessageBox.Show("Sucessfuly made examination appointment", "Success");
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error");
        }
    }
}