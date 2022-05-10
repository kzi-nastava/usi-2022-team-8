using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Renovations.Repository;
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
    /// Interaction logic for RoomsTableWindow.xaml
    /// </summary>
    public partial class RoomsTableWindow : Window
    {
        private RoomRepository _roomRepository = RoomRepository.GetInstance();
        public RoomsTableWindow()
        {
            InitializeComponent();
            LoadRows();
        }

        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Room> rooms = _roomRepository.GetActiveRooms();
            foreach (Room room in rooms)
            {
                dataGrid.Items.Add(room);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddRoomDialog addRoomDialog = new AddRoomDialog();
            addRoomDialog.ShowDialog();

            LoadRows();
            dataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Room selectedRoom = (Room)dataGrid.SelectedItem;
            if (selectedRoom.Type == RoomType.Warehouse)
            {
                System.Windows.MessageBox.Show("You cant edit warehouse!", "Edit error", MessageBoxButton.OK, MessageBoxImage.Error);
                dataGrid.SelectedItem = null;
                return;
            }
       
            EditRoomDialog editRoomDialog = new EditRoomDialog(selectedRoom);
            editRoomDialog.ShowDialog();
            dataGrid.SelectedItem = null;
            dataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Room selectedRoom = (Room)dataGrid.SelectedItem;
            if (selectedRoom.Type == RoomType.Warehouse)
            {
                System.Windows.MessageBox.Show("You cant delete warehouse!", "Edit error", MessageBoxButton.OK, MessageBoxImage.Error);
                dataGrid.SelectedItem = null;
                return;
            }
            if (!CheckOccurrenceOfRoom(selectedRoom))
            {
                System.Windows.MessageBox.Show("You cant delete room because of scheduled connections!", "Edit error", MessageBoxButton.OK, MessageBoxImage.Error);
                dataGrid.SelectedItem = null;
                return;
            }

            if (System.Windows.MessageBox.Show("Are you sure you want to delete selected room", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                dataGrid.Items.Remove(selectedRoom);
                if (CheckRenovationStatus(selectedRoom))
                {
                    _roomRepository.Delete(selectedRoom.Id);
                }

            }
        }

        private bool CheckRenovationStatus(Room selectedRoom)
        {
            RenovationRepository renovationRepository = RenovationRepository.GetInstance();
            foreach (Renovation renovation in renovationRepository.Renovations)
            {
                if (renovation.Room == selectedRoom)
                {
                    selectedRoom.IsActive = false;
                    return false;
                }
                if (renovation.GetType() == typeof(RoomMerger))
                {
                    RoomMerger roomMerger = (RoomMerger)renovation;
                    if (roomMerger.RoomForMerge == selectedRoom)
                    {
                        selectedRoom.IsActive = false;
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckOccurrenceOfRoom(Room selectedRoom)
        {
            EquipmentTransferRepository equipmentTransferRepository = EquipmentTransferRepository.GetInstance();
            if (equipmentTransferRepository.EquipmentTransfers.Find(eqTransfer => eqTransfer.FromRoom == selectedRoom || eqTransfer.ToRoom == selectedRoom) != null)
            {
                return false;
            }

            ExaminationRepository examinationRepository = ExaminationRepository.GetInstance();
            if (examinationRepository.Examinations.Find(examination => examination.Room == selectedRoom) != null)
            {
                return false;
            }

            OperationRepository operationRepository = OperationRepository.GetInstance();
            if (operationRepository.Operations.Find(operation => operation.Room == selectedRoom) != null)
            {
                return false;
            }

            return true;
        }
    }
}
