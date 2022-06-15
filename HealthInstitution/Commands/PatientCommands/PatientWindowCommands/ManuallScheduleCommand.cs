using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.GUI.PatientWindows;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.PatientWindowCommands;

public class ManuallScheduleCommand : CommandBase
{
    private Patient _loggedPatient;

    public ManuallScheduleCommand(Patient loggedPatient)
    {
        _loggedPatient = loggedPatient;
    }

    public override void Execute(object? parameter)
    {
        new PatientScheduleWindow(this._loggedPatient)
        {
            DataContext = new PatientScheduleWindowViewModel(_loggedPatient)
        }.ShowDialog();
    }
}