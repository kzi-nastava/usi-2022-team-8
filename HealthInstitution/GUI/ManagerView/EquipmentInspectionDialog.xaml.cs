using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms.Model;
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
        }

        private void EquipmentTypeCheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            equipmentTypeCheckBox = sender as System.Windows.Controls.CheckBox;
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }

        private void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }
    }
}
