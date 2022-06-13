using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;
using System.Windows;
using System.Windows.Controls;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for ConsumedEquipmentDialog.xaml
    /// </summary>
    public partial class ConsumedEquipmentDialog : Window
    {
        private Room _room;

        public ConsumedEquipmentDialog(Room room)
        {
            this._room = room;
            InitializeComponent();
            roomLabel.Content = _room;
        }

        private void EquipmentComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var equipmentComboBox = sender as System.Windows.Controls.ComboBox;
            var dynamicEquipment = RoomService.GetDynamicEquipment(_room);
            foreach (var equipment in dynamicEquipment)
            {
                equipmentComboBox.Items.Add(equipment);
            }
        }

        private void EquipmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var equipment = (Equipment)equipmentComboBox.SelectedItem;
            consumedQuantityComboBox.Items.Clear();
            if (equipment != null)
            {
                for (int i = 0; i <= equipment.Quantity; i++)
                    consumedQuantityComboBox.Items.Add(i);
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var equipment = (Equipment)equipmentComboBox.SelectedItem;
                var consumedQuantity = (int)consumedQuantityComboBox.SelectedItem;
                EquipmentService.RemoveConsumed(equipment, consumedQuantity);
                System.Windows.MessageBox.Show("You have removed " + consumedQuantity + " " + equipment.Name + " from " + _room, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                equipmentComboBox.Items.Remove(equipment);
                consumedQuantityComboBox.Items.Clear();
                if (consumedQuantityComboBox.Items.Count == 0)
                {
                    System.Windows.MessageBox.Show("You have updated all equipment quantites", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("You have to choose equipment and quantity!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("You have updated equipment quantites", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}