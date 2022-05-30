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
            List<dynamic> rows = GetMissingEquipment();
            foreach(dynamic row in rows)
            {
                dataGrid.Items.Add(row);
                
            }
            dataGrid.Items.Refresh();
        }
        int GetQuantityOfEquipmentInRoom(Room room, Equipment equipment)
        {
            int quantity = 0;
            foreach(Equipment e in room.AvailableEquipment)
            {
                if (e.Name == equipment.Name)
                    quantity += e.Quantity;
            }
            return quantity;
        }
        private dynamic MakePair(Room room, Equipment equipment, int quantityInRoom)
        {
            dynamic obj = new ExpandoObject();
            obj.Room = room;
            obj.Equipment = equipment.Name;
            obj.Quantity = quantityInRoom;
            return obj;
        }
        void CheckRoomEquipmentPair(Room room, Equipment equipment, HashSet<String> distinctEquipments, List<dynamic> pairs)
        {
            if (equipment.IsDynamic && !distinctEquipments.Contains(equipment.Name))
            {
                distinctEquipments.Add(equipment.Name);
                int quantityInRoom = GetQuantityOfEquipmentInRoom(room, equipment);
                if (room.Id != 1 && quantityInRoom < 5)
                {
                    pairs.Add(MakePair(room, equipment, quantityInRoom));
                }
            }
        }
        private List<dynamic> GetMissingEquipment()
        {
            List<Equipment> equipments = EquipmentRepository.GetInstance().Equipments;
            List<Room> rooms = RoomRepository.GetInstance().Rooms;
            List<dynamic> pairs=new();
            HashSet<string> distinctEquipments;
            foreach (Room room in rooms)
            {
                distinctEquipments = new();
                foreach(Equipment equipment in equipments)
                {
                    CheckRoomEquipmentPair(room, equipment, distinctEquipments, pairs);
                }
            }
            return pairs;
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
