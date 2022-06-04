using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.EquipmentTransfers.Functionality;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace HealthInstitution.GUI.SecretaryView
{
    /// <summary>
    /// Interaction logic for DynamicEquipmentTransferDialog.xaml
    /// </summary>
    public partial class DynamicEquipmentTransferDialog : Window
    {
        Room _toRoom;
        string _equipmentName;
        public DynamicEquipmentTransferDialog(Room toRoom, string equipmentName)
        {
            _toRoom=toRoom;
            _equipmentName=equipmentName;
            InitializeComponent();
        }
        private void ProcessDialog()
        {
            System.Windows.MessageBox.Show("Transfer is completed successfully", "Dynamic equipment transfer", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
            quantityBox.Clear();
            roomComboBox.SelectedItem = null;
        }
        private void RoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<Room> rooms = RoomService.GetAll();

            foreach (Room fromRoom in rooms)
            {
                Equipment? equipment = EquipmentService.GetEquipmentFromRoom(fromRoom, _equipmentName);
                if (fromRoom != _toRoom && equipment != null && equipment.Quantity > 5)
                {
                    roomComboBox.Items.Add(fromRoom + " (has " + equipment.Quantity + ")");
                }
            }
        }
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string? roomText = (string)roomComboBox.SelectedItem;
                Room? fromRoom = RoomService.GetRoomFromString(roomText);
                if (fromRoom != null)
                {
                    TransferDynamicEquipment(fromRoom);
                    ProcessDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("Room must be selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
        private void TransferDynamicEquipment(Room fromRoom)
        {
            int quantity = EquipmentService.GetQuantityForTransfer(quantityBox.Text, fromRoom, _equipmentName);
            Equipment equipment = EquipmentService.GetEquipmentFromRoom(fromRoom, _equipmentName);
            EquipmentTransferService.Transfer(_toRoom, equipment, quantity);

        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
