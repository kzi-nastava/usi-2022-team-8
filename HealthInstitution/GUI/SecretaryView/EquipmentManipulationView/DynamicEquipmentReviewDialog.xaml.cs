using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using System.Dynamic;
using System.Windows;

namespace HealthInstitution.GUI.SecretaryView
{
    /// <summary>
    /// Interaction logic for DynamicEquipmentReviewDialog.xaml
    /// </summary>
    public partial class DynamicEquipmentReviewDialog : Window
    {
        public DynamicEquipmentReviewDialog()
        {
            InitializeComponent();
            LoadRows();
        }

        private void LoadRows()
        {
            List<dynamic> rows = EquipmentService.GetMissingEquipment();
            foreach(dynamic row in rows)
            {
                dataGrid.Items.Add(row);
            }
            dataGrid.Items.Refresh();
        }       
        private void selectEquipment_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedEquipment = (dynamic)dataGrid.SelectedItem;
            if (selectedEquipment != null)
            {
                DynamicEquipmentTransferDialog dynamicEquipmentTransferDialog = new DynamicEquipmentTransferDialog(selectedEquipment.Room, selectedEquipment.Equipment);
                dynamicEquipmentTransferDialog.ShowDialog();
                dataGrid.SelectedItem = null;
                LoadRows();

            }
        }
    }
}
