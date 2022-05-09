using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ArrangeEquipmentForSplitWindow()
        {
            InitializeComponent();
        }

        private void TransferToFirst_Click(object sender, RoutedEventArgs e)
        {
            EquipmentTransferForSplitDialog equipmentTransferForSplitDialog = new EquipmentTransferForSplitDialog();
            equipmentTransferForSplitDialog.ShowDialog();
        }


        private void TransferToSecond_Click(object sender, RoutedEventArgs e)
        {
            EquipmentTransferForSplitDialog equipmentTransferForSplitDialog = new EquipmentTransferForSplitDialog();
            equipmentTransferForSplitDialog.ShowDialog();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
