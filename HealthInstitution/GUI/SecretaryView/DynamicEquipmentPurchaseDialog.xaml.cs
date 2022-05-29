using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
    /// Interaction logic for DynamicEquipmentPurchaseDialog.xaml
    /// </summary>
    public partial class DynamicEquipmentPurchaseDialog : Window
    {
        static EquipmentRepository _equipmentRepository = EquipmentRepository.GetInstance();
        public DynamicEquipmentPurchaseDialog()
        {
            InitializeComponent();
        }
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int quantity = Int32.Parse(quantityBox.Text);
                string? equipmentName = (string)equipmentComboBox.SelectedItem;

                if (equipmentName != null)
                {
                    EquipmentType equipmentType = GetEquipmentType(equipmentName);

                    EquipmentDTO selectedEquipment = new EquipmentDTO(quantity, equipmentName, equipmentType, true);
                    ScheduleWarehouseRefill(selectedEquipment);
                    quantityBox.Clear();
                    equipmentComboBox.SelectedItem = null;
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

        private EquipmentType GetEquipmentType(string equipmentName)
        {
            EquipmentType equipmentType = EquipmentType.AppointmentEquipment;
            foreach (Equipment equipment in _equipmentRepository.Equipments)
            {
                if (equipment.Name == equipmentName)
                    equipmentType = equipment.Type;
            }
            return equipmentType;
        }

        private void ScheduleWarehouseRefill(EquipmentDTO equipmentDTO)
        {
            Equipment newEquipment = _equipmentRepository.Add(equipmentDTO);

            DateTime tomorrowSameTime = DateTime.Now + new TimeSpan(1, 0, 0, 0);
            Room warehouse = RoomRepository.GetInstance().RoomById[1];
            EquipmentTransferDTO equipmentTransferDTO = new EquipmentTransferDTO(newEquipment, null, warehouse, tomorrowSameTime);
            EquipmentTransferRepository.GetInstance().Add(equipmentTransferDTO);

            System.Windows.MessageBox.Show("Request for warehouse refill is created successfully", "Warehouse refill", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        
        private void EquipmentComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> equipmentPerQuantity = EquipmentRepository.GetInstance().EquipmentPerQuantity;
            
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
