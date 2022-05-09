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

namespace HealthInstitution.GUI.ManagerView
{
    /// <summary>
    /// Interaction logic for EquipmentTransferDialog.xaml
    /// </summary>
    public partial class EquipmentTransferDialog : Window
    {
        private RoomRepository _roomRepository = RoomRepository.GetInstance();
        private EquipmentRepository _equipmentRepository = EquipmentRepository.GetInstance(); 
        private EquipmentTransferRepository _equipmentTransferRepository = EquipmentTransferRepository.GetInstance();
        public EquipmentTransferDialog()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FromRoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {         
            fromRoomComboBox.ItemsSource = _roomRepository.GetActiveRooms();
            fromRoomComboBox.SelectedItem = null;
        }

        private void ToRoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            toRoomComboBox.ItemsSource = _roomRepository.GetActiveRooms();
            toRoomComboBox.SelectedItem = null;
        }

        private void EquipmentComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            equipmentComboBox.IsEnabled = false;
        }

        private void FromRoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            equipmentComboBox.IsEnabled = true;
            Room selectedRoomFrom = (Room)fromRoomComboBox.SelectedItem;
            equipmentComboBox.ItemsSource = selectedRoomFrom.AvailableEquipment;
            toRoomComboBox.SelectedItem = null;
        }

        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCompleteness())
            {
                System.Windows.MessageBox.Show("You need to select all data in form!", "Failed transfer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DateTime date = (DateTime)transferDate.SelectedDate;
            if (date < DateTime.Today)
            {
                System.Windows.MessageBox.Show("You need to select future date!", "Failed transfer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Room fromRoom = (Room)fromRoomComboBox.SelectedItem;
            Room toRoom = (Room)toRoomComboBox.SelectedItem;
            if (fromRoom == toRoom)
            {
                System.Windows.MessageBox.Show("You cant transfer equipment to same room!", "Failed transfer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int quantity = Int32.Parse(quantityBox.Text);
            Equipment equipment = (Equipment)equipmentComboBox.SelectedItem;
            if (quantity > equipment.Quantity)
            {
                System.Windows.MessageBox.Show("You cant transfer more equipment than room has!", "Failed transfer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int projectedQuantityLoss = CalculateProjectedQuantityLoss(fromRoom, equipment);
            if (quantity > equipment.Quantity - projectedQuantityLoss)
            {
                System.Windows.MessageBox.Show("You cant transfer more equipment than room has because of projected transfers!", "Failed transfer", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (date == DateTime.Today)
            {
                EquipmentTransferChecker.Transfer(toRoom, equipment, quantity);
                System.Windows.MessageBox.Show("Equipment transfer completed!", "Equipment transfer", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Equipment newEquipment = _equipmentRepository.Add(quantity, equipment.Name, equipment.Type, equipment.IsDynamic);
                _equipmentTransferRepository.Add(newEquipment, fromRoom, toRoom, date);
                System.Windows.MessageBox.Show("Equipment transfer scheduled!", "Equipment transfer", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.Close();

        }
       
        private int CalculateProjectedQuantityLoss(Room fromRoom, Equipment equipment)
        {
            int projectedQuantityLoss = 0;
            foreach (EquipmentTransfer equipmentTransfer in _equipmentTransferRepository.EquipmentTransfers)
            {
                if (equipmentTransfer.FromRoom == fromRoom)
                {
                    if (equipmentTransfer.Equipment.Name == equipment.Name && equipmentTransfer.Equipment.Type == equipment.Type)
                    {
                        projectedQuantityLoss += equipmentTransfer.Equipment.Quantity;
                    }
                }
            }
            return projectedQuantityLoss;
        }

        private bool CheckCompleteness()
        {
            if (fromRoomComboBox.SelectedItem == null)
            {
                return false;
            }
            if (toRoomComboBox.SelectedItem == null)
            {
                return false;
            }
            if (equipmentComboBox.SelectedItem == null)
            {
                return false;
            }
            if (quantityBox.Text.Trim()=="")
            {
                return false;
            }
            if (transferDate.SelectedDate == null)
            {
                return false;
            }
            return true;
        }
    }
}
