using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.ViewModels.GUIViewModels.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.PatientWindowCommands;

public class RateHospitalCommand : CommandBase
{
    public override void Execute(object? parameter)
    {
        var window = DIContainer.GetService<PatientHospitalPollDialog>();
        window.ShowDialog();
    }
}