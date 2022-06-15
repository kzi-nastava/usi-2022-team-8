using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Renovations;
using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Renovations.Repository;
using HealthInstitution.Core.Rooms;
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
        IRoomService _roomService;
        public RoomsTableWindow(IRoomService roomService)
        {
            InitializeComponent();
            _roomService = roomService;
            LoadRows();
        }

        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Room> rooms = _roomService.GetActive();
            foreach (Room room in rooms)
            {
                dataGrid.Items.Add(room);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddRoomDialog addRoomDialog = new AddRoomDialog(DIContainer.GetService<IRoomService>());
            addRoomDialog.ShowDialog();

            LoadRows();
            dataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Room selectedRoom = (Room)dataGrid.SelectedItem;
            if (selectedRoom.IsWarehouse())
            {
                System.Windows.MessageBox.Show("You cant edit warehouse!", "Edit error", MessageBoxButton.OK, MessageBoxImage.Error);
                dataGrid.SelectedItem = null;
                return;
            }
       
            EditRoomDialog editRoomDialog = new EditRoomDialog(selectedRoom, DIContainer.GetService<IRoomService>());
            editRoomDialog.ShowDialog();
            dataGrid.SelectedItem = null;
            dataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Room selectedRoom = (Room)dataGrid.SelectedItem;
            if (selectedRoom.IsWarehouse())
            {
                System.Windows.MessageBox.Show("You cant delete warehouse!", "Edit error", MessageBoxButton.OK, MessageBoxImage.Error);
                dataGrid.SelectedItem = null;
                return;
            }
            if (_roomService.CheckImportantOccurrenceOfRoom(selectedRoom))
            {
                System.Windows.MessageBox.Show("You cant delete room because of scheduled connections!", "Delete error", MessageBoxButton.OK, MessageBoxImage.Error);
                dataGrid.SelectedItem = null;
                return;
            }

            if (System.Windows.MessageBox.Show("Are you sure you want to delete selected room", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                dataGrid.Items.Remove(selectedRoom);
                _roomService.MoveRoomToRenovationHistory(selectedRoom);
                
            }
        }
 
    }
}
