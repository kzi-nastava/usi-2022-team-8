using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.DoctorPickCommands;

public class NameSearchCommand : CommandBase
{
    private DoctorPickViewModel _viewModel;

    public NameSearchCommand(DoctorPickViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        _viewModel.Doctors = DoctorService.SearchByName(_viewModel.SearchTerm);
        _viewModel.LoadRows();
    }
}