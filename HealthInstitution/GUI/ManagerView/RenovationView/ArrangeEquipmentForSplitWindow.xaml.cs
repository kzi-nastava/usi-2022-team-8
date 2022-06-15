using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.EquipmentTransfers;
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

namespace HealthInstitution.GUI.ManagerView.RenovationView
{
    /// <summary>
    /// Interaction logic for ArrangeEquipmentForSplitWindow.xaml
    /// </summary>
    public partial class ArrangeEquipmentForSplitWindow : Window
    {
        private List<Equipment> _firstRoomEquipments;
        private List<Equipment> _secondRoomEquipments;
        public ArrangeEquipmentForSplitWindow()
        {
            InitializeComponent();
        }
        public void SetEquipmentCollections(List<Equipment> firstRoomEquipment, List<Equipment> secondRoomEquipments)
        {
            _firstRoomEquipments = firstRoomEquipment;
            _secondRoomEquipments = secondRoomEquipments;
            Load();
        }

        private void Load()
        {
            firstRoomDataGrid.Items.Clear();
            foreach (var item in _firstRoomEquipments)
            {
                firstRoomDataGrid.Items.Add(item);
            }

            secondRoomDataGrid.Items.Clear();
            foreach (var item in _secondRoomEquipments)
            {
                secondRoomDataGrid.Items.Add(item);
            }
            CheckTransferOptions();
        }

        private void CheckTransferOptions()
        {
            if (_firstRoomEquipments == null || !_firstRoomEquipments.Any())
            {
                transferToSecondButton.IsEnabled = false;
            } else
            {
                transferToSecondButton.IsEnabled = true;
            }

            if (_secondRoomEquipments == null || !_secondRoomEquipments.Any())
            {
                transferToFirstButton.IsEnabled = false;
            }
            else
            {
                transferToFirstButton.IsEnabled = true;
            }
        }


        private void TransferToFirst_Click(object sender, RoutedEventArgs e)
        {
            EquipmentTransferForSplitDialog equipmentTransferForSplitDialog = DIContainer.GetService<EquipmentTransferForSplitDialog>();
            equipmentTransferForSplitDialog.SetEquipmentCollections(_secondRoomEquipments, _firstRoomEquipments);
            equipmentTransferForSplitDialog.ShowDialog();

            Load();
            firstRoomDataGrid.Items.Refresh();
            secondRoomDataGrid.Items.Refresh();

        }


        private void TransferToSecond_Click(object sender, RoutedEventArgs e)
        {
            EquipmentTransferForSplitDialog equipmentTransferForSplitDialog = DIContainer.GetService<EquipmentTransferForSplitDialog>();
            equipmentTransferForSplitDialog.SetEquipmentCollections(_firstRoomEquipments, _secondRoomEquipments);
            equipmentTransferForSplitDialog.ShowDialog();

            Load();
            firstRoomDataGrid.Items.Refresh();
            secondRoomDataGrid.Items.Refresh();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
