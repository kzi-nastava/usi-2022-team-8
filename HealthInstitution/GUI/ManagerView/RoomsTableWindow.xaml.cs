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
        RoomRepository roomRepository = RoomRepository.GetInstance();
        public RoomsTableWindow()
        {
            InitializeComponent();
            LoadGridRows();
        }

        public void LoadGridRows()
        {
            dataGrid.Items.Clear();
            List<Room> rooms = roomRepository.rooms;
            foreach (Room room in rooms)
            {
                dataGrid.Items.Add(room);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddRoomDialog addRoomDialog = new AddRoomDialog();
            addRoomDialog.ShowDialog();

            LoadGridRows();
            dataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Room selectedRoom = (Room)dataGrid.SelectedItem;
            if (selectedRoom.type == RoomType.Warehouse)
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
            if (selectedRoom.type == RoomType.Warehouse)
            {
                System.Windows.MessageBox.Show("You cant delete warehouse!", "Edit error", MessageBoxButton.OK, MessageBoxImage.Error);
                dataGrid.SelectedItem = null;
                return;
            }

            if (System.Windows.MessageBox.Show("Are you sure you want to delete selected room", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                dataGrid.Items.Remove(selectedRoom);
                roomRepository.DeleteRoom(selectedRoom.id);
            }
        }
    }
}
