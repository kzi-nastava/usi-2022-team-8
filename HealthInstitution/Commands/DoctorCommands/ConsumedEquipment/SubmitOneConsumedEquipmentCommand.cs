using HealthInstitution.Core;
using HealthInstitution.Core.Equipments;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.ConsumedEquipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.ConsumedEquipment
{
    internal class SubmitOneConsumedEquipmentCommand : CommandBase
    {
        private ConsumedEquipmentDialogViewModel _consumedEquipmentDialogViewModel;
        IEquipmentService _equipmentService;
        public SubmitOneConsumedEquipmentCommand(ConsumedEquipmentDialogViewModel consumedEquipmentDialogViewModel, IEquipmentService equipmentService)
        {
            _consumedEquipmentDialogViewModel = consumedEquipmentDialogViewModel;
            _equipmentService = equipmentService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                var equipment = _consumedEquipmentDialogViewModel.GetEquipment();
                var consumedQuantity = _consumedEquipmentDialogViewModel.GetConsumedQuantity();
                _equipmentService.RemoveConsumed(equipment, consumedQuantity);
                System.Windows.MessageBox.Show("You have removed " + consumedQuantity + " " + equipment.Name + " from " + _consumedEquipmentDialogViewModel.Room.ToString(), "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                _consumedEquipmentDialogViewModel.EquipmentComboBoxItems.Remove(equipment);
                _consumedEquipmentDialogViewModel.ConsumedQuantity = string.Empty;
                if (_consumedEquipmentDialogViewModel.EquipmentComboBoxItems.Count == 0)
                {
                    System.Windows.MessageBox.Show("You have updated all equipment quantites", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    //_consumedEquipmentDialogViewModel.ThisWindow.Close();
                }
                _consumedEquipmentDialogViewModel.ConsumedQuantity = "";
            }
            catch
            {
                System.Windows.MessageBox.Show("You have to choose equipment and quantity!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}