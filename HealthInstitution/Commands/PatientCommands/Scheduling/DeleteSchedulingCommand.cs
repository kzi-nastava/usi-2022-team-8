using HealthInstitution.Core;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.PatientCommands.Scheduling;

public class DeleteSchedulingCommand : CommandBase
{
    private PatientScheduleWindowViewModel _patientScheduleWindowViewModel;
    IExaminationService _examinationService;
    ITrollCounterService _trollCounterService;
    IScheduleEditRequestsService _scheduleEditRequestsService;
    public DeleteSchedulingCommand(PatientScheduleWindowViewModel patientScheduleWindowViewModel, IExaminationService examinationService,ITrollCounterService trollCounterService, IScheduleEditRequestsService scheduleEditRequestsService)
    {
        _patientScheduleWindowViewModel = patientScheduleWindowViewModel;
        _examinationService = examinationService;
        _trollCounterService = trollCounterService;
        _scheduleEditRequestsService = scheduleEditRequestsService;
    }

    public override void Execute(object? parameter)
    {
        Examination selectedExamination = _patientScheduleWindowViewModel.GetSelectedExamination();
        _trollCounterService.TrollCheck(_patientScheduleWindowViewModel.LoggedPatient.Username);
        _patientScheduleWindowViewModel.RefreshGrid();
        _trollCounterService.AppendEditDeleteDates(_patientScheduleWindowViewModel.LoggedPatient.Username);
        ConfirmDelete(selectedExamination);
    }

    private bool IsConfirmedDelete()
    {
        return System.Windows.MessageBox.Show("Are you sure you want to delete selected examination", "Question",
            MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
    }

    private void ConfirmDelete(Examination selectedExamination)
    {
        if (IsConfirmedDelete())
        {
            if (selectedExamination.Appointment.AddDays(-2) < DateTime.Now)
            {
                _scheduleEditRequestsService.AddDeleteRequest(selectedExamination);
            }
            else
            {
                _examinationService.Delete(selectedExamination.Id);
                selectedExamination.Doctor.Examinations.Remove(selectedExamination);
                _patientScheduleWindowViewModel.RefreshGrid();
            }
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return _patientScheduleWindowViewModel.SelectedExaminationIndex >= 0 && base.CanExecute(parameter);
    }
}