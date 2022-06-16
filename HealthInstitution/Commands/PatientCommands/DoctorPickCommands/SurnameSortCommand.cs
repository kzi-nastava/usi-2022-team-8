using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.DoctorPickCommands;

public class SurnameSortCommand : CommandBase
{
    private DoctorPickViewModel _viewModel;
    IDoctorService _doctorService;
    public SurnameSortCommand(DoctorPickViewModel viewModel, IDoctorService doctorService)
    {
        _viewModel = viewModel;
        _doctorService = doctorService;
    }

    public override void Execute(object? parameter)
    {
        _viewModel.Doctors = _doctorService.OrderByDoctorSurname(_viewModel.Doctors);
        _viewModel.LoadRows();
    }
}