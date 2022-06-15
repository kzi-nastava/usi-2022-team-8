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

    public DeleteSchedulingCommand(PatientScheduleWindowViewModel patientScheduleWindowViewModel)
    {
        _patientScheduleWindowViewModel = patientScheduleWindowViewModel;
    }

    public override void Execute(object? parameter)
    {
        Examination selectedExamination = _patientScheduleWindowViewModel.GetSelectedExamination();
        TrollCounterService.TrollCheck(_patientScheduleWindowViewModel.LoggedPatient.Username);
        _patientScheduleWindowViewModel.RefreshGrid();
        TrollCounterService.AppendEditDeleteDates(_patientScheduleWindowViewModel.LoggedPatient.Username);
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
                ScheduleEditRequestService.AddDeleteRequest(selectedExamination);
            }
            else
            {
                ExaminationService.Delete(selectedExamination.Id);
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