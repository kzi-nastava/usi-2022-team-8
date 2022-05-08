using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
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

namespace HealthInstitution.GUI.ManagerView
{
    /// <summary>
    /// Interaction logic for EquipmentTableWindow.xaml
    /// </summary>
    
    public partial class EquipmentTableWindow : Window
    {
        public EquipmentTableWindow(List<TableItemEquipment> items)
        {
            InitializeComponent();
            load(items);
        }

        private void load(List<TableItemEquipment> items)
        {
            dataGrid.Items.Clear();
            foreach (var item in items)
            {
                dataGrid.Items.Add(item);
            }
        }
    }
}
