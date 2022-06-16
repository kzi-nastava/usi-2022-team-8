using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.DoctorPickCommands;

public class SurnameSortCommand : CommandBase
{
    private DoctorPickViewModel _viewModel;

    public SurnameSortCommand(DoctorPickViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        _viewModel.Doctors = DoctorService.OrderByDoctorSurname(_viewModel.Doctors);
        _viewModel.LoadRows();
    }
}