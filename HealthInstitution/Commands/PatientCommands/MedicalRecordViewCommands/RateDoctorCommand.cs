using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
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
    }

    public override void Execute(object? parameter)
    {
        new DoctorPollDialog(_medicalRecordViewModel.Examinations[_medicalRecordViewModel.SelectedExaminationIndex].Doctor)
        {
            DataContext = new DoctorPollViewModel(_medicalRecordViewModel.Examinations[_medicalRecordViewModel.SelectedExaminationIndex].Doctor)
        }.ShowDialog();
    }

    public override bool CanExecute(object? parameter)
    {
        return _medicalRecordViewModel.SelectedExaminationIndex >= 0 && base.CanExecute(parameter);
    }
}