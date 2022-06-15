using HealthInstitution.Core;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.RecommendedScheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.RecommendedSchedulingCommands;

public class ClosestFitCommand : CommandBase
{
    private ClosestFitViewModel _viewModel;

    public ClosestFitCommand(ClosestFitViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        ExaminationService.Add(ExaminationService.ParseExaminationToExaminationDTO(_viewModel.GetChosen()));
    }
}