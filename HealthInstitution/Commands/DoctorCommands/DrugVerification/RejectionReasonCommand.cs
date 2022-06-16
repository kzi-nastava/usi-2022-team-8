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
    internal class RejectionReasonCommand : CommandBase
    {
        private RejectionReasonDialogViewModel _rejectionReasonDialogViewModel;
        IDrugVerificationService _drugVerificationService;
        public RejectionReasonCommand(RejectionReasonDialogViewModel rejectionReasonDialogViewModel, IDrugVerificationService drugVerificationService)
        {
            _rejectionReasonDialogViewModel = rejectionReasonDialogViewModel;
            _drugVerificationService = drugVerificationService;
        }

        public override void Execute(object? parameter)
        {
            var rejectionReason = _rejectionReasonDialogViewModel.RejectionReason;
            if (rejectionReason.Trim() == "")
            {
                System.Windows.MessageBox.Show("You have to write a reason for rejection!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _drugVerificationService.Reject(_rejectionReasonDialogViewModel.SelectedDrug, rejectionReason);
                System.Windows.MessageBox.Show("Successfull rejection!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
