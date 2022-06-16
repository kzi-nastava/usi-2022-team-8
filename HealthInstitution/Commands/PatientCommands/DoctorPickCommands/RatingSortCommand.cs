using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.DoctorPickCommands;

public class RatingSortCommand : CommandBase
{
    private DoctorPickViewModel _viewModel;
    IDoctorService _doctorService;
    public RatingSortCommand(DoctorPickViewModel viewModel, IDoctorService doctorService)
    {
        _viewModel = viewModel;
        _doctorService = doctorService;
    }

    public override void Execute(object? parameter)
    {
        _viewModel.Doctors = _doctorService.OrderByDoctorRating(_viewModel.Doctors);
        _viewModel.LoadRows();
    }
}