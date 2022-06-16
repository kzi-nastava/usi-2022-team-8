using HealthInstitution.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.ConsumedEquipment
{
    internal class FinishConsumedEquipmentCommand : CommandBase
    {
        public FinishConsumedEquipmentCommand(ViewModels.GUIViewModels.DoctorViewViewModels.ConsumedEquipment.ConsumedEquipmentDialogViewModel consumedEquipmentDialogViewModel)
        {
        }

        public override void Execute(object? parameter)
        {
            System.Windows.MessageBox.Show("You have updated equipment quantites", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
    }
