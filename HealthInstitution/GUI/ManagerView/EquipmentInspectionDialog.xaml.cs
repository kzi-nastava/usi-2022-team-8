using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms;
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

        private RoomRepository _roomRepository = RoomRepository.GetInstance();
        public EquipmentInspectionDialog()
        {
            InitializeComponent();
        }

        private void QuantityComboBox_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void Quantity_Checked(object sender, RoutedEventArgs e)
        {
            quantityComboBox.IsEnabled = true;
        }

        private void Quantity_Unchecked(object sender, RoutedEventArgs e)
        {
            quantityComboBox.IsEnabled = false;
            quantityComboBox.SelectedItem = null;
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

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCompleteness())
            {
                System.Windows.MessageBox.Show("You need to select item in menu!", "Failed filter", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            EquipmentFilterDTO equipmentFilterDTO = FormEquipmentFilterDTO();
            
            List<TableItemEquipment> items = EquipmentService.FilterEquipment(equipmentFilterDTO);
            
            if (items == null || !items.Any())
            {
                System.Windows.MessageBox.Show("No search results!", "Failed search", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            this.Close();
            EquipmentTableWindow equipmentTableWindow = new EquipmentTableWindow(items);
            equipmentTableWindow.ShowDialog();
        }

        private EquipmentFilterDTO FormEquipmentFilterDTO()
        {
            bool applyRoomTypeFilter = (bool)roomTypeCheckBox.IsChecked;
            RoomType roomTypeFilter;
            if (applyRoomTypeFilter)
            {
                roomTypeFilter = (RoomType)roomTypeComboBox.SelectedItem;
            }
            else
            {
                roomTypeFilter = (RoomType)0;
            }


            bool applyEquipmentTypeFilter = (bool)equipmentTypeCheckBox.IsChecked;
            EquipmentType equipmentTypeFilter;
            if (applyEquipmentTypeFilter)
            {
                equipmentTypeFilter = (EquipmentType)equipmentTypeComboBox.SelectedItem;
            }
            else
            {
                equipmentTypeFilter = (EquipmentType)0;
            }

            bool applyQuantityFilter = (bool)quantityCheckBox.IsChecked;
            int quantityFilter = quantityComboBox.SelectedIndex;

            return new EquipmentFilterDTO(applyRoomTypeFilter, roomTypeFilter, applyEquipmentTypeFilter, equipmentTypeFilter, applyQuantityFilter, quantityFilter);
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

            List<TableItemEquipment> items = SearchEquipment(searchInput);
            
            if (items == null || !items.Any())
            {
                System.Windows.MessageBox.Show("No search results!", "Failed search", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            this.Close();
            EquipmentTableWindow equipmentTableWindow = new EquipmentTableWindow(items);
            equipmentTableWindow.ShowDialog();
        }

        private List<TableItemEquipment> SearchEquipment(string searchInput)
        {
            List<TableItemEquipment> items = new List<TableItemEquipment>();
            List<Room> rooms = _roomRepository.GetActive();
            foreach (Room room in rooms)
            {
                foreach (Equipment equipment in room.AvailableEquipment)
                {
                    if (SearchMatch(room, equipment, searchInput))
                    {
                        TableItemEquipment equipmentByRoom = new TableItemEquipment(room, equipment);
                        items.Add(equipmentByRoom);
                    }
                }
            }
            return items;
        }

        private bool SearchMatch(Room room, Equipment equipment, string searchInput)
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

        private void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            List<TableItemEquipment> items = LoadRows();
            if (items == null || !items.Any())
            {
                System.Windows.MessageBox.Show("There is no equipment!", "No equipment", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            this.Close();
            EquipmentTableWindow equipmentTableWindow = new EquipmentTableWindow(items);
            equipmentTableWindow.ShowDialog();
        }

        private List<TableItemEquipment> LoadRows()
        {
            List<TableItemEquipment> items = new List<TableItemEquipment>();
            List<Room> rooms = _roomRepository.GetActive();
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
