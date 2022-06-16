using HealthInstitution.Core;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.SchedulePerforming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.ExaminationPerforming
{
    internal class AddAllergenCommand : CommandBase
    {
        private PerformExaminationDialogViewModel _performExaminationDialogViewModel;

        public AddAllergenCommand(PerformExaminationDialogViewModel performExaminationDialogViewModel)
        {
            _performExaminationDialogViewModel = performExaminationDialogViewModel;
        }

        public override void Execute(object? parameter)
        {
            string allergen = _performExaminationDialogViewModel.Allergen;
            if (allergen != "")
            {
                _performExaminationDialogViewModel.Allergens.Add(allergen);
                _performExaminationDialogViewModel.Allergen = String.Empty;
            }
        }
    }
}