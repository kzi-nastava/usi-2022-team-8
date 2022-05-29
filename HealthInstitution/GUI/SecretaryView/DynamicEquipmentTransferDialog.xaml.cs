using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.EquipmentTransfers.Functionality;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
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

namespace HealthInstitution.GUI.SecretaryView
{
    /// <summary>
    /// Interaction logic for DynamicEquipmentTransferDialog.xaml
    /// </summary>
    public partial class DynamicEquipmentTransferDialog : Window
    {
        Room ToRoom;
        string EquipmentName;
        static EquipmentRepository _equipmentRepository = EquipmentRepository.GetInstance();
        static RoomRepository _roomRepository=RoomRepository.GetInstance();

        public DynamicEquipmentTransferDialog(Room toRoom, string equipmentName)
        {
            ToRoom=toRoom;
            EquipmentName=equipmentName;
            InitializeComponent();
        }
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Room? fromRoom = (Room)roomComboBox.SelectedItem;
                Room? fromRoom = GetRoomFromStringInComboBox();
                if (fromRoom != null)
                {
                    int quantity = GetQuantity();
                    Equipment equipment = GetEquipmentFromRoom(fromRoom);
                    TransferDynamicEquipment(quantity, equipment);
                    quantityBox.Clear();
                    roomComboBox.SelectedItem = null;
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
        private Room? GetRoomFromStringInComboBox()
        {
            string? record = (string)roomComboBox.SelectedItem;
            if (record == null)
                return null;
            
            string[] tokens = record.Split(' ');
            string type = tokens[0], number=tokens[1];
            List<Room> rooms = _roomRepository.Rooms;
            foreach (Room room in rooms)
            {
                if (room.Type.ToString() == type && room.Number.ToString()==number)
                    return room;
            }
            return null;
        }
        private int GetQuantity()
        {
            int quantity;
            string exceptionMessage = "Quantity must be filled";
            try
            {
                quantity = Int32.Parse(quantityBox.Text);
                Equipment equipment = GetEquipmentFromRoom(GetRoomFromStringInComboBox());
                if (quantity + 5 > equipment.Quantity)
                {
                    exceptionMessage = "You can't transfer this quantity of equipment. There must be at least 5 items left in the room.";
                    throw new Exception();
                }
                return quantity;
            }
            catch
            {
                throw new Exception(exceptionMessage);
            }
        }
        private Equipment GetEquipmentFromRoom(Room room)
        {
            foreach (Equipment equipment in room.AvailableEquipment)
                if (equipment.Name == EquipmentName)
                    return equipment;
            return null;
        }
        private void TransferDynamicEquipment(int quantity, Equipment equipment)
        {
            EquipmentTransferChecker.Transfer(ToRoom, equipment, quantity);
            System.Windows.MessageBox.Show("Transfer is completed successfully", "Dynamic equipment transfer", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }

        private void RoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<Room> rooms = _roomRepository.Rooms;
            
            foreach (Room fromRoom in rooms)
            {
                Equipment? equipment=GetEquipmentFromRoom(fromRoom);
                if (fromRoom != ToRoom && equipment != null && equipment.Quantity > 5)
                {
                    roomComboBox.Items.Add(fromRoom+" (has "+equipment.Quantity+")"); // konverzija samo sobe, bez ostatka teksta
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
