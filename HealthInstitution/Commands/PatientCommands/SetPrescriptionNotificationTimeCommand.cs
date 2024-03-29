﻿using HealthInstitution.Core;
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

public class SetPrescriptionNotificationTimeCommand : CommandBase
{
    private PrescriptionNotificationSettingViewModel _viewModel;
    IPrescriptionNotificationService _prescriptionNotificationService;
    public SetPrescriptionNotificationTimeCommand(PrescriptionNotificationSettingViewModel viewModel, IPrescriptionNotificationService prescriptionNotificationService)
    {
        _viewModel = viewModel;
        _prescriptionNotificationService = prescriptionNotificationService;
    }

    public override void Execute(object? parameter)
    {
        Prescription prescription = _viewModel.GetSelectedPrescription();
        DateTime before = _viewModel.GetbeforeTime();
        PrescriptionNotificationSettings recepieNotificationSettings = new PrescriptionNotificationSettings(before, _viewModel.LoggedPatient.Username, prescription, DateTime.Now, prescription.Id);
        _prescriptionNotificationService.UpdateSettings(recepieNotificationSettings.Id, recepieNotificationSettings);
        List<DateTime> dateTimes = _prescriptionNotificationService.GenerateDateTimes(recepieNotificationSettings);
        _prescriptionNotificationService.GenerateCronJobs(dateTimes, recepieNotificationSettings, _viewModel.LoggedPatient.Username);
    }
}