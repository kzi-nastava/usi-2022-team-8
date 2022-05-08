using HealthInstitution.Core.Equipments.Model;
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
    /// Interaction logic for EquipmentInspectionDialog.xaml
    /// </summary>
    public partial class EquipmentInspectionDialog : Window
    {
        private System.Windows.Controls.ComboBox _roomTypeComboBox;
        private System.Windows.Controls.ComboBox _quantityComboBox;
        private System.Windows.Controls.ComboBox _equipmentTypeComboBox;

        private System.Windows.Controls.CheckBox _roomTypeCheckBox;
        private System.Windows.Controls.CheckBox _quantityCheckBox;
        private System.Windows.Controls.CheckBox _equipmentTypeCheckBox;

        private RoomRepository _roomRepository = RoomRepository.GetInstance();
        public EquipmentInspectionDialog()
        {
            InitializeComponent();
        }

        private void quantityComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            _quantityComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> items = new List<String>();
            items.Add("out of stock");
            items.Add("0-10");
            items.Add("10+");

            _quantityComboBox.ItemsSource = items;
            _quantityComboBox.SelectedItem = null;
            _quantityComboBox.IsEnabled = false;
        }

        private void roomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            _roomTypeComboBox = sender as System.Windows.Controls.ComboBox;
            List<RoomType> items = new List<RoomType>();
            items.Add(RoomType.OperatingRoom);
            items.Add(RoomType.ExaminationRoom);
            items.Add(RoomType.RestRoom);
            items.Add(RoomType.Warehouse);

            _roomTypeComboBox.ItemsSource = items;
            _roomTypeComboBox.SelectedItem = null;
            _roomTypeComboBox.IsEnabled = false;
        }

        private void equipmentTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            _equipmentTypeComboBox = sender as System.Windows.Controls.ComboBox;
            List<EquipmentType> items = new List<EquipmentType>();
            items.Add(EquipmentType.AppointmentEquipment);
            items.Add(EquipmentType.SurgeryEquipment);
            items.Add(EquipmentType.RoomFurniture);
            items.Add(EquipmentType.HallEquipment);

            _equipmentTypeComboBox.ItemsSource = items;
            _equipmentTypeComboBox.SelectedItem = null;
            _equipmentTypeComboBox.IsEnabled = false;
        }

        private void roomType_Checked(object sender, RoutedEventArgs e)
        {
            _roomTypeComboBox.IsEnabled=true;
        }

        private void roomType_Unchecked(object sender, RoutedEventArgs e)
        {
            _roomTypeComboBox.IsEnabled = false;
            _roomTypeComboBox.SelectedItem = null;
        }

        private void roomTypeCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            _roomTypeCheckBox = sender as System.Windows.Controls.CheckBox;
        }
        private void quantity_Checked(object sender, RoutedEventArgs e)
        {
            _quantityComboBox.IsEnabled = true;
        }

        private void quantity_Unchecked(object sender, RoutedEventArgs e)
        {
            _quantityComboBox.IsEnabled = false;
            _quantityComboBox.SelectedItem = null;
        }

        private void quantityCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            _quantityCheckBox = sender as System.Windows.Controls.CheckBox;
        }
        private void equipmentType_Checked(object sender, RoutedEventArgs e)
        {
            _equipmentTypeComboBox.IsEnabled = true;      
        }

        private void equipmentType_Unchecked(object sender, RoutedEventArgs e)
        {
            _equipmentTypeComboBox.IsEnabled = false;
            _equipmentTypeComboBox.SelectedItem = null;
        }

        private void equipmentTypeCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            _equipmentTypeCheckBox = sender as System.Windows.Controls.CheckBox;
        }

        private void filter_Click(object sender, RoutedEventArgs e)
        {
            if (!checkCompleteness())
            {
                System.Windows.MessageBox.Show("You need to select item in menu!", "Failed filter", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<TableItemEquipment> items = new List<TableItemEquipment>();
            List<Room> rooms = _roomRepository.Rooms;
            foreach (Room room in rooms)
            {
                if (!matchRoomType(room))
                    continue;
                foreach (Equipment equipment in room.AvailableEquipment)
                {
                    if (!matchEquipmentType(equipment))
                        continue;
                    if (!matchQuantity(equipment))
                        continue;
                    
                    TableItemEquipment equipmentByRoom = new TableItemEquipment(room, equipment);
                    items.Add(equipmentByRoom);
                }
            }
            if (items == null || !items.Any())
            {
                System.Windows.MessageBox.Show("No search results!", "Failed search", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            this.Close();
            EquipmentTableWindow equipmentTableWindow = new EquipmentTableWindow(items);
            equipmentTableWindow.ShowDialog();
        }

        private bool matchQuantity(Equipment equipment)
        {
            if (!(bool)_quantityCheckBox.IsChecked)
            {
                return true;
            }
            int selectedIdx = _quantityComboBox.SelectedIndex;
            switch (selectedIdx)
            {
                case 0:
                    if (equipment.Quantity != 0)
                        return false;
                    break;
                case 1:
                    if (equipment.Quantity > 10)
                        return false;
                    break;
                case 2:
                    if (equipment.Quantity < 10)
                        return false;
                    break;
            }
            return true;
        }

        private bool matchEquipmentType(Equipment equipment)
        {
            if (!(bool)_equipmentTypeCheckBox.IsChecked)
            {
                return true;
            }
            EquipmentType filteredEquipmentType = (EquipmentType)_equipmentTypeComboBox.SelectedItem;
            if (equipment.Type == filteredEquipmentType)
            {
                return true;
            }
            return false;
        }

        private bool matchRoomType(Room room)
        {
            if (!(bool)_roomTypeCheckBox.IsChecked)
            {
                return true;
            }
            RoomType filteredRoomType = (RoomType)_roomTypeComboBox.SelectedItem;
            if (room.Type == filteredRoomType)
            {
                return true;
            }
            return false;
        }

        private bool checkCompleteness()
        {
            if((bool)_equipmentTypeCheckBox.IsChecked && _equipmentTypeComboBox.SelectedItem == null)
            {
                return false;
            }
            if ((bool)_roomTypeCheckBox.IsChecked && _roomTypeComboBox.SelectedItem == null)
            {
                return false;
            }
            if ((bool)_quantityCheckBox.IsChecked && _quantityComboBox.SelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            string searchInput = SearchBox.Text;
            if (searchInput.Trim() == "")
            {
                System.Windows.MessageBox.Show("Some search characters need to be placed!", "Failed search", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List <TableItemEquipment> items = new List<TableItemEquipment>();
            List<Room> rooms = _roomRepository.Rooms;
            foreach (Room room in rooms)
            {
                foreach (Equipment equipment in room.AvailableEquipment)
                {
                    if (searchMatch(room, equipment, searchInput))
                    {
                        TableItemEquipment equipmentByRoom = new TableItemEquipment(room, equipment);
                        items.Add(equipmentByRoom);
                    }
                }
            }
            if (items == null || !items.Any())
            {
                System.Windows.MessageBox.Show("No search results!", "Failed search", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            this.Close();
            EquipmentTableWindow equipmentTableWindow = new EquipmentTableWindow(items);
            equipmentTableWindow.ShowDialog();
        }

        private bool searchMatch(Room room, Equipment equipment, string searchInput)
        {
            if (room.Type.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            if (room.Number.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            if (equipment.Type.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            if (equipment.Name.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            return false;
        }

        private void viewAll_Click(object sender, RoutedEventArgs e)
        {
            List<TableItemEquipment> items = loadRows();
            if (items == null || !items.Any())
            {
                System.Windows.MessageBox.Show("There is no equipment!", "No equipment", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            this.Close();
            EquipmentTableWindow equipmentTableWindow = new EquipmentTableWindow(items);
            equipmentTableWindow.ShowDialog();
        }

        private List<TableItemEquipment> loadRows()
        {
            List<TableItemEquipment> items = new List<TableItemEquipment>();
            List<Room> rooms = _roomRepository.Rooms;
            foreach (Room room in rooms)
            {
                foreach (Equipment equipment in room.AvailableEquipment)
                {                   
                    TableItemEquipment equipmentByRoom = new TableItemEquipment(room, equipment);
                    items.Add(equipmentByRoom);                    
                }
            }
            return items;
        }
    }
}
