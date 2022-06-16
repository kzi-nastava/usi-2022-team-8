using HealthInstitution.Core;
using HealthInstitution.Core.Drugs;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.DrugVerification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.DrugVerification
{
    internal class AcceptDrugCommand : CommandBase
    {
        private DrugsVerificationTableViewModel _drugsVerificationTableView;

        public AcceptDrugCommand(DrugsVerificationTableViewModel drugsVerificationTableView)
        {
            _drugsVerificationTableView = drugsVerificationTableView;
        }

        public override void Execute(object? parameter)
        {
            System.Windows.MessageBox.Show("You have accepted a new drug!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            DrugVerificationService.Accept(_drugsVerificationTableView.GetSelectedDrug());
            _drugsVerificationTableView.RefreshGrid();
        }
    }
}
