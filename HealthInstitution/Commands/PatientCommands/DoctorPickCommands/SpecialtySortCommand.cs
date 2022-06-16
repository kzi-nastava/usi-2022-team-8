using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.DoctorPickCommands;

public class SpecialitySortCommand : CommandBase
{
    private DoctorPickViewModel _viewModel;
    IDoctorService _doctorService;
    public SpecialitySortCommand(DoctorPickViewModel viewModel, IDoctorService doctorService)
    {
        _doctorService = doctorService;
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        _viewModel.Doctors = _doctorService.OrderByDoctorSpeciality(_viewModel.Doctors);
        _viewModel.LoadRows();
    }
}
