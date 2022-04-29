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
        public EquipmentTransferDialog()
        {
            InitializeComponent();
        }

        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FromRoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            //todo
        }

        private void ToRoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            //todo
        }

        private void EquipmentComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            //todo
        }
    }
}
