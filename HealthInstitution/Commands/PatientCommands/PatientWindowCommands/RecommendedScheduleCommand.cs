﻿using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.RecommendedScheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.PatientWindowCommands;

public class RecommendedScheduleCommand : CommandBase
{
    private Patient _loggedPatient;

    public RecommendedScheduleCommand(Patient loggedPatient)
    {
        _loggedPatient = loggedPatient;
    }

    public override void Execute(object? parameter)
    {
        var window = DIContainer.GetService<RecommendedWindow>();
        window.SetLoggedPatient(_loggedPatient);
        window.ShowDialog();
    }
}