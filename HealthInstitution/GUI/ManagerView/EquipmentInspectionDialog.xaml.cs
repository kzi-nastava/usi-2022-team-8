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
        private System.Windows.Controls.ComboBox roomTypeComboBox;
        private System.Windows.Controls.ComboBox quantityComboBox;
        private System.Windows.Controls.ComboBox equipmentTypeComboBox;

        private System.Windows.Controls.CheckBox roomTypeCheckBox;
        private System.Windows.Controls.CheckBox quantityCheckBox;
        private System.Windows.Controls.CheckBox equipmentTypeCheckBox;

        RoomRepository roomRepository = RoomRepository.GetInstance();
        public EquipmentInspectionDialog()
        {
            InitializeComponent();
        }

        private void QuantityComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            quantityComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> items = new List<String>();
            items.Add("out of stock");
            items.Add("0-10");
            items.Add("10+");

            quantityComboBox.ItemsSource = items;
            quantityComboBox.SelectedItem = null;
            quantityComboBox.IsEnabled = false;
        }

        private void RoomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            roomTypeComboBox = sender as System.Windows.Controls.ComboBox;
            List<RoomType> items = new List<RoomType>();
            items.Add(RoomType.OperatingRoom);
            items.Add(RoomType.ExaminationRoom);
            items.Add(RoomType.RestRoom);
            items.Add(RoomType.Warehouse);

            roomTypeComboBox.ItemsSource = items;
            roomTypeComboBox.SelectedItem = null;
            roomTypeComboBox.IsEnabled = false;
        }

        private void EquipmentTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            equipmentTypeComboBox = sender as System.Windows.Controls.ComboBox;
            List<EquipmentType> items = new List<EquipmentType>();
            items.Add(EquipmentType.AppointmentEquipment);
            items.Add(EquipmentType.SurgeryEquipment);
            items.Add(EquipmentType.RoomFurniture);
            items.Add(EquipmentType.HallEquipment);

            equipmentTypeComboBox.ItemsSource = items;
            equipmentTypeComboBox.SelectedItem = null;
            equipmentTypeComboBox.IsEnabled = false;
        }

        private void RoomType_Checked(object sender, RoutedEventArgs e)
        {
            roomTypeComboBox.IsEnabled=true;
        }

        private void RoomType_Unchecked(object sender, RoutedEventArgs e)
        {
            roomTypeComboBox.IsEnabled = false;
            roomTypeComboBox.SelectedItem = null;
        }

        private void RoomTypeCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            roomTypeCheckBox = sender as System.Windows.Controls.CheckBox;
        }
        private void Quantity_Checked(object sender, RoutedEventArgs e)
        {
            quantityComboBox.IsEnabled = true;
        }

        private void Quantity_Unchecked(object sender, RoutedEventArgs e)
        {
            quantityComboBox.IsEnabled = false;
            quantityComboBox.SelectedItem = null;
        }

        private void QuantityCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            quantityCheckBox = sender as System.Windows.Controls.CheckBox;
        }
        private void EquipmentType_Checked(object sender, RoutedEventArgs e)
        {
            equipmentTypeComboBox.IsEnabled = true;      
        }

        private void EquipmentType_Unchecked(object sender, RoutedEventArgs e)
        {
            equipmentTypeComboBox.IsEnabled = false;
            equipmentTypeComboBox.SelectedItem = null;
        }

        private void EquipmentTypeCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            equipmentTypeCheckBox = sender as System.Windows.Controls.CheckBox;
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCompleteness())
            {
                System.Windows.MessageBox.Show("You need to select item in menu!", "Failed filter", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<TableItemEquipment> items = new List<TableItemEquipment>();
            List<Room> rooms = roomRepository.rooms;
            foreach (Room room in rooms)
            {
                if (!MatchRoomType(room))
                    continue;
                foreach (Equipment equipment in room.availableEquipment)
                {
                    if (!MatchEquipmentType(equipment))
                        continue;
                    if (!MatchQuantity(equipment))
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

        private bool MatchQuantity(Equipment equipment)
        {
            if (!(bool)quantityCheckBox.IsChecked)
            {
                return true;
            }
            int selectedIdx = quantityComboBox.SelectedIndex;
            switch (selectedIdx)
            {
                case 0:
                    if (equipment.quantity != 0)
                        return false;
                    break;
                case 1:
                    if (equipment.quantity > 10)
                        return false;
                    break;
                case 2:
                    if (equipment.quantity < 10)
                        return false;
                    break;
            }
            return true;
        }

        private bool MatchEquipmentType(Equipment equipment)
        {
            if (!(bool)equipmentTypeCheckBox.IsChecked)
            {
                return true;
            }
            EquipmentType filteredEquipmentType = (EquipmentType)equipmentTypeComboBox.SelectedItem;
            if (equipment.type == filteredEquipmentType)
            {
                return true;
            }
            return false;
        }

        private bool MatchRoomType(Room room)
        {
            if (!(bool)roomTypeCheckBox.IsChecked)
            {
                return true;
            }
            RoomType filteredRoomType = (RoomType)roomTypeComboBox.SelectedItem;
            if (room.type == filteredRoomType)
            {
                return true;
            }
            return false;
        }

        private bool CheckCompleteness()
        {
            if((bool)equipmentTypeCheckBox.IsChecked && equipmentTypeComboBox.SelectedItem == null)
            {
                return false;
            }
            if ((bool)roomTypeCheckBox.IsChecked && roomTypeComboBox.SelectedItem == null)
            {
                return false;
            }
            if ((bool)quantityCheckBox.IsChecked && quantityComboBox.SelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchInput = SearchBox.Text;
            if (searchInput.Trim() == "")
            {
                System.Windows.MessageBox.Show("Some search characters need to be placed!", "Failed search", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List <TableItemEquipment> items = new List<TableItemEquipment>();
            List<Room> rooms = roomRepository.rooms;
            foreach (Room room in rooms)
            {
                foreach (Equipment equipment in room.availableEquipment)
                {
                    if (SearchMatch(room, equipment, searchInput))
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

        private bool SearchMatch(Room room, Equipment equipment, string searchInput)
        {
            if (room.type.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            if (room.number.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            if (equipment.type.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            if (equipment.name.ToString().ToLower().Contains(searchInput.ToLower()))
                return true;
            return false;
        }

        private void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            List<TableItemEquipment> items = LoadAllEquipments();
            if (items == null || !items.Any())
            {
                System.Windows.MessageBox.Show("There is no equipment!", "No equipment", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            this.Close();
            EquipmentTableWindow equipmentTableWindow = new EquipmentTableWindow(items);
            equipmentTableWindow.ShowDialog();
        }

        private List<TableItemEquipment> LoadAllEquipments()
        {
            List<TableItemEquipment> items = new List<TableItemEquipment>();
            List<Room> rooms = roomRepository.rooms;
            foreach (Room room in rooms)
            {
                foreach (Equipment equipment in room.availableEquipment)
                {                   
                    TableItemEquipment equipmentByRoom = new TableItemEquipment(room, equipment);
                    items.Add(equipmentByRoom);                    
                }
            }
            return items;
        }
    }
}
