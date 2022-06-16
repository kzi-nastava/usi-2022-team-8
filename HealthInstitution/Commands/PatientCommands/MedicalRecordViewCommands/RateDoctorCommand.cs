using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Polls;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
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
    private IPollService _pollService;

    public RateDoctorCommand(MedicalRecordViewViewModel medicalRecordViewModel, IPollService pollService)
    {
        _medicalRecordViewModel = medicalRecordViewModel;
        _pollService = pollService;
        _medicalRecordViewModel.PropertyChanged += _medicalRecordViewModel_PropertyChanged;
    }

    private void _medicalRecordViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_medicalRecordViewModel.SelectedExaminationIndex))
            OnCanExecuteChanged();
    }

    public override void Execute(object? parameter)
    {
        Examination examination = _medicalRecordViewModel.Examinations[_medicalRecordViewModel.SelectedExaminationIndex];
        var window = DIContainer.GetService<DoctorPollDialog>();
        window.SetRatedDoctor(examination.Doctor);
        window.ShowDialog();
        _pollService.AddRatedExamination(examination.Id);
        _medicalRecordViewModel.SelectedExaminationIndex = 0;
    }

    public override bool CanExecute(object? parameter)
    {
        return !_pollService.IsRatedExamination(_medicalRecordViewModel.Examinations[_medicalRecordViewModel.SelectedExaminationIndex].Id) && base.CanExecute(parameter);
    }
}