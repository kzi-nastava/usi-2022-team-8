using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using System;
using System.Collections.Generic;
using System.Dynamic;
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

namespace HealthInstitution.GUI.SecretaryView
{
    /// <summary>
    /// Interaction logic for DynamicEquipmentTransferDialog.xaml
    /// </summary>
    public partial class DynamicEquipmentTransferDialog : Window
    {
        public DynamicEquipmentTransferDialog()
        {
            InitializeComponent();
            LoadRows();
        }

        private void LoadRows()
        {
            List<dynamic> rows = GetMissingEquipment();
            foreach(dynamic row in rows)
            {
                dataGrid.Items.Add(row);
                
            }
            dataGrid.Items.Refresh();
        }
        private List<dynamic> GetMissingEquipment()
        {
            List<Equipment> equipments = EquipmentRepository.GetInstance().Equipments;
            List<Room> rooms = RoomRepository.GetInstance().Rooms;
            List<dynamic> pairs=new List<dynamic>();
            foreach(Room room in rooms)
            {
                foreach(Equipment equipment in equipments)
                {
                    /*if(room.Id!=1 && room.Nesto<5)
                    {
                        dynamic obj=new ExpandoObject();
                        obj.Room = room;
                        obj.Equipment = equipment;
                        obj.Quantity = room.Nesto;
                        pairs.Add(obj);
                    }*/
                }
            }
            return pairs;
        }

        private void selectEquipment_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
