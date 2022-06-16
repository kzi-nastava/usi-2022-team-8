using HealthInstitution.Core;
using HealthInstitution.Core.Examinations;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands;

public class DoctorSortCommand : CommandBase
{
    private MedicalRecordViewViewModel _medicalRecordViewModel;
    IExaminationService _examinationService;
    public DoctorSortCommand(MedicalRecordViewViewModel medicalRecordViewModel, IExaminationService examinationService)
    {
        _examinationService = examinationService;
        _medicalRecordViewModel = medicalRecordViewModel;
    }

    public override void Execute(object? parameter)
    {
        _medicalRecordViewModel.Examinations = _examinationService.OrderByDoctor(_medicalRecordViewModel.Examinations);
        _medicalRecordViewModel.PutIntoGrid();
    }
}