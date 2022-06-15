using HealthInstitution.Core;
using HealthInstitution.Core.PrescriptionNotifications.Model;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.PrescriptionNotificationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands;

public class SetPrescriptionNotificationTime : CommandBase
{
    private PrescriptionNotificationSettingViewModel _viewModel;

    public override void Execute(object? parameter)
    {
        Prescription prescription = _prescriptions[dataGrid.SelectedIndex];
        DateTime before = DateTime.Today;
        before = before.AddMinutes(_minutes).AddHours(_hours);
        PrescriptionNotificationSettings recepieNotificationSettings = new PrescriptionNotificationSettings(before, _loggedPatinet, prescription, DateTime.Now, prescription.Id);
        PrescriptionNotificationService.UpdateSettings(recepieNotificationSettings.Id, recepieNotificationSettings);
        List<DateTime> dateTimes = PrescriptionNotificationService.GenerateDateTimes(recepieNotificationSettings);
        PrescriptionNotificationService.GenerateCronJobs(dateTimes, recepieNotificationSettings, _viewModel.Log _loggedPatinet);
    }
}