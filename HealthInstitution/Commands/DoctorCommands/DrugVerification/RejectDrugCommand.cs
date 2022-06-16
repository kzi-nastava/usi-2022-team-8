using HealthInstitution.Core;
using HealthInstitution.Core.Drugs;
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.DrugVerification;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.DrugVerification
{
    public class RejectDrugCommand : CommandBase
    {
        private DrugsVerificationTableViewModel _drugsVerificationTableView;

        public RejectDrugCommand(DrugsVerificationTableViewModel drugsVerificationTableView)
        {
            _drugsVerificationTableView = drugsVerificationTableView;
        }

        public override void Execute(object? parameter)
        {
            new DrugRejectionReasonDialog(_drugsVerificationTableView.GetSelectedDrug()).ShowDialog();
            _drugsVerificationTableView.RefreshGrid();
        }
    }
}
