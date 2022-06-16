using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Polls;
using HealthInstitution.GUI.PatientView.Polls;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;
using HealthInstitution.ViewModels.GUIViewModels.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.MedicalRecordViewCommands;

public class RateDoctorCommand : CommandBase
{
    private MedicalRecordViewViewModel _medicalRecordViewModel;

    public RateDoctorCommand(MedicalRecordViewViewModel medicalRecordViewModel)
    {
        _medicalRecordViewModel = medicalRecordViewModel;

        _medicalRecordViewModel.PropertyChanged += _medicalRecordViewModel_PropertyChanged;
    }

    private void _medicalRecordViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_medicalRecordViewModel.SelectedExaminationIndex))
            OnCanExecuteChanged();
    }

    public override void Execute(object? parameter)
    {
        new DoctorPollDialog(_medicalRecordViewModel.Examinations[_medicalRecordViewModel.SelectedExaminationIndex].Doctor)
        {
            DataContext = new DoctorPollViewModel(_medicalRecordViewModel.Examinations[_medicalRecordViewModel.SelectedExaminationIndex].Doctor)
        }.ShowDialog();
        PollService.AddRatedExamination(_medicalRecordViewModel.Examinations[_medicalRecordViewModel.SelectedExaminationIndex].Id);
    }

    public override bool CanExecute(object? parameter)
    {
        return !PollService.IsRatedExamination(_medicalRecordViewModel.Examinations[_medicalRecordViewModel.SelectedExaminationIndex].Id) && base.CanExecute(parameter);
    }
}