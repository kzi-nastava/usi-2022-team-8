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
    private IExaminationService _examinationService;

    public ClosestFitCommand(ClosestFitViewModel viewModel, IExaminationService examinationService)
    {
        _viewModel = viewModel;
        _examinationService = examinationService;
    }

    public override void Execute(object? parameter)
    {
        _examinationService.Add(_examinationService.ParseExaminationToExaminationDTO(_viewModel.GetChosen()));
        _viewModel.ThisWindow.Close();
    }
}