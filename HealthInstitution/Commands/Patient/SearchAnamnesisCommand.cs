using HealthInstitution.Core;
using HealthInstitution.Core.Examinations;
using HealthInstitution.GUI.PatientViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.Patient;

public class SearchAnamnesisCommand : CommandBase
{
    private MedicalRecordViewModel _medicalRecordViewModel;

    public SearchAnamnesisCommand(MedicalRecordViewModel medicalRecordViewModel)
    {
        _medicalRecordViewModel = medicalRecordViewModel;
    }

    public override void Execute(object? parameter)
    {
        _medicalRecordViewModel.Examinations =
             ExaminationService.GetSearchAnamnesis(_medicalRecordViewModel.Keyword, _medicalRecordViewModel.LoggedPatient.Username);
        _medicalRecordViewModel.PutIntoGrid();
    }
}