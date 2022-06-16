using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.PatientWindowCommands;

public class MedicalRecordViewCommand : CommandBase
{
    private User _loggedPatient;
    public MedicalRecordViewCommand(User loggedPatient)
    {
        _loggedPatient = loggedPatient;
    }

    public override void Execute(object? parameter)
    {
        var window = DIContainer.GetService<MedicalRecordView>();
        window.SetLoggedPatient(_loggedPatient);
        window.ShowDialog();
    }
}