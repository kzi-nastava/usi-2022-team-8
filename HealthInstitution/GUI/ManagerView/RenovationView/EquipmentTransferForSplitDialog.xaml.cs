using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.EquipmentTransfers.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthInstitution.GUI.ManagerView.RenovationView
{
    /// <summary>
    /// Interaction logic for EquipmentTransferForSplitDialog.xaml
    /// </summary>
    public partial class EquipmentTransferForSplitDialog : Window
    {
        private List<Equipment> _firstRoomEquipments;
        private List<Equipment> _secondRoomEquipments;
        IEquipmentTransferService _equipmentTransferService;
        public EquipmentTransferForSplitDialog(IEquipmentTransferService equipmentTransferService)
        {
            InitializeComponent();
            _equipmentTransferService = equipmentTransferService;
        }
        public void SetEquipmentCollections(List<Equipment> firstRoomEquipment, List<Equipment> secondRoomEquipments)
        {
            _firstRoomEquipments = firstRoomEquipment;
            _secondRoomEquipments = secondRoomEquipments;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void EquipmentComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            equipmentComboBox.ItemsSource = _firstRoomEquipments;
            equipmentComboBox.SelectedItem = null;
        }

        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCompleteness())
            {
                System.Windows.MessageBox.Show("You need to select all data in form!", "Failed transfer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            int quantity = Int32.Parse(quantityBox.Text);
            Equipment equipment = (Equipment)equipmentComboBox.SelectedItem;

            if (!ValidateQuantity(equipment, quantity))
            {
                return;
            }
 
            _equipmentTransferService.Transfer(_secondRoomEquipments, equipment, quantity);
            System.Windows.MessageBox.Show("Equipment transfer completed!", "Equipment transfer", MessageBoxButton.OK, MessageBoxImage.Information);
            
            this.Close();
        }

        private bool ValidateQuantity(Equipment equipment, int quantity)
        {
            if (quantity > equipment.Quantity)
            {
                System.Windows.MessageBox.Show("You cant transfer more equipment than room has!", "Failed transfer", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool CheckCompleteness()
        {
            
            if (equipmentComboBox.SelectedItem == null)
            {
                return false;
            }
            if (quantityBox.Text.Trim() == "")
            {
                return false;
            }
            
            return true;
        }


    }
}
