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
            List<Room> rooms = roomRepository.rooms;
            foreach (Room room in rooms)
            {
                Console.WriteLine(room);
                dataGrid.Items.Add(room);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            AddRoomDialog addRoomDialog = new AddRoomDialog();
            addRoomDialog.ShowDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Room selectedRoom = (Room)dataGrid.SelectedItem;
            if (selectedRoom.type == RoomType.Warehouse)
            {
                System.Windows.MessageBox.Show("You cant edit warehouse!", "Edit error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.Close();
            EditRoomDialog editRoomDialog = new EditRoomDialog(selectedRoom);
            editRoomDialog.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }
    }
}
