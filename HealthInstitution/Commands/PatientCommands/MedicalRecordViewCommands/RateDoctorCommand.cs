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
    IPollService _pollService;
    public RateDoctorCommand(MedicalRecordViewViewModel medicalRecordViewModel, IPollService pollService)
    {
        _pollService = pollService;
        _medicalRecordViewModel = medicalRecordViewModel;
    }

    public override void Execute(object? parameter)
    {
        var window = DIContainer.GetService<DoctorPollDialog>();
        Doctor doctor = _medicalRecordViewModel.Examinations[_medicalRecordViewModel.SelectedExaminationIndex].Doctor;
        window.SetRatedDoctor(doctor);
        window.DataContext = new DoctorPollViewModel(doctor, _pollService);
        window.ShowDialog();
        
    }

    public override bool CanExecute(object? parameter)
    {
        return _medicalRecordViewModel.SelectedExaminationIndex >= 0 && base.CanExecute(parameter);
    }
}