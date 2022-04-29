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
        public EquipmentInspectionDialog()
        {
            InitializeComponent();
        }

        private void QuantityComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var quantityComboBox = sender as System.Windows.Controls.ComboBox;
            List<String> items = new List<String>();
            items.Add("out of stock");
            items.Add("0-10");
            items.Add("10+");

            quantityComboBox.ItemsSource = items;
            quantityComboBox.SelectedIndex = 0;
        }

        private void RoomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var roomTypeComboBox = sender as System.Windows.Controls.ComboBox;
            List<RoomType> items = new List<RoomType>();
            items.Add(RoomType.OperatingRoom);
            items.Add(RoomType.ExaminationRoom);
            items.Add(RoomType.RestRoom);
            items.Add(RoomType.Warehouse);

            roomTypeComboBox.ItemsSource = items;
            roomTypeComboBox.SelectedIndex = 0;
        }

        private void EquipmentTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var equipmentTypeComboBox = sender as System.Windows.Controls.ComboBox;
            List<EquipmentType> items = new List<EquipmentType>();
            items.Add(EquipmentType.AppointmentEquipment);
            items.Add(EquipmentType.SurgeryEquipment);
            items.Add(EquipmentType.RoomFurniture);
            items.Add(EquipmentType.HallEquipment);

            equipmentTypeComboBox.ItemsSource = items;
            equipmentTypeComboBox.SelectedIndex = 0;
        }
    }
}
