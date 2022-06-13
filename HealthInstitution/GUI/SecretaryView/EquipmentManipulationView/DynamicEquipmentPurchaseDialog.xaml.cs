using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace HealthInstitution.GUI.SecretaryView
{
    /// <summary>
    /// Interaction logic for DynamicEquipmentPurchaseDialog.xaml
    /// </summary>
    public partial class DynamicEquipmentPurchaseDialog : Window
    {
        IEquipmentTransferService _equipmentTransferService;
        IEquipmentService _equipmentService;
        public DynamicEquipmentPurchaseDialog(IEquipmentTransferService equipmentTransferService, IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
            _equipmentTransferService = equipmentTransferService;
            InitializeComponent();
        }
        private void ProcessDialog()
        {
            System.Windows.MessageBox.Show("Request for warehouse refill is created successfully", "Warehouse refill", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadEquipmentComboBox();
            quantityBox.Clear();
            equipmentComboBox.SelectedItem = null;
        }
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int quantity = Int32.Parse(quantityBox.Text);
                string? equipmentName = (string)equipmentComboBox.SelectedItem;
                if (equipmentName != null)
                {
                    _equipmentTransferService.ScheduleWarehouseRefill(equipmentName, quantity);
                    ProcessDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("Equipment must be selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            catch
            {
                System.Windows.MessageBox.Show("Quantity must be filled", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private void EquipmentComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            LoadEquipmentComboBox();
        }

        private void LoadEquipmentComboBox()
        {
            equipmentComboBox.Items.Clear();
            Dictionary<string, int> equipmentPerQuantity = _equipmentService.GetEquipmentPerQuantity();
            foreach (var equipment in equipmentPerQuantity)
            {
                if (equipment.Value == 0)
                {
                    equipmentComboBox.Items.Add(equipment.Key);
                }
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
