using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.RecommendedScheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.RecommendedSchedulingCommands;

internal class FirstFitScheduleCommand : CommandBase
{
    private RecommendedWindowViewModel _viewModel;

    public FirstFitScheduleCommand(RecommendedWindowViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        var fitDTO = GenerateFirstFitDTO();
        bool found = RecommendedSchedulingService.FindFirstFit(fitDTO);
        if (!found)
        {
            var closestFitDTO = GenerateClosestFitDTO();
            List<Examination> suggestions =
                RecommendedSchedulingService.FindClosestFit(closestFitDTO);
            new ClosestFit()
            {
                DataContext = new ClosestFitViewModel(suggestions)
            }.ShowDialog();
        }
    }

    private RecommendedSchedulingDTOs GenerateFirstFitDTO()
    {
        DateTime startDateTime = _viewModel.GetStartDateTime();
        DateTime endDateTime = _viewModel.GetEndDateTime();
        return new RecommendedSchedulingDTOs(startDateTime.Hour, startDateTime.Minute, endDateTime.Date, endDateTime.Hour, endDateTime.Minute,
            23, _viewModel.LoggedPatient.Username, _viewModel.GetDoctorUsername());
    }

    private ClosestFitDTO GenerateClosestFitDTO()
    {
        DateTime startDateTime = _viewModel.GetStartDateTime();
        DateTime endDateTime = _viewModel.GetEndDateTime();
        return new ClosestFitDTO(startDateTime.Hour, startDateTime.Minute, startDateTime.Date, endDateTime.Hour, endDateTime.Minute,
            23, _viewModel.LoggedPatient.Username, _viewModel.GetDoctorUsername(), (_viewModel.Priority as String) == "Doctor");
    }
}