using HealthInstitution.Core;
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.DoctorCommands.DoctorWindow
{
    internal class DrugManagementCommand : CommandBase
    {
        public DrugManagementCommand()
        {
        }

        public override void Execute(object? parameter)
        {
            new DrugsVerificationTable().ShowDialog();
        }
    }
}
