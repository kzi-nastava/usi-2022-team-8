using HealthInstitution.Core;
using HealthInstitution.Core.Examinations;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.MedicalRecordViewCommands
{
    public class SpecializationSortCommand : CommandBase
    {
        private MedicalRecordViewViewModel _medicalRecordViewModel;

        public SpecializationSortCommand(MedicalRecordViewViewModel medicalRecordViewModel)
        {
            _medicalRecordViewModel = medicalRecordViewModel;
        }

        public override void Execute(object? parameter)
        {
            _medicalRecordViewModel.Examinations = ExaminationService.OrderByDoctorSpeciality(_medicalRecordViewModel.Examinations);
            _medicalRecordViewModel.PutIntoGrid();
        }
    }
}