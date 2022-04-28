using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
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
    /// Interaction logic for EditRoomDialog.xaml
    /// </summary>
    public partial class EditRoomDialog : Window
    {
        private Room room;
        RoomRepository roomRepository = RoomRepository.GetInstance();
        public EditRoomDialog(Room room)
        {
            InitializeComponent();
            this.room = room;
            SetRoomData();
        }

        private void SetRoomData()
        {
            numberBox.Text = room.number.ToString();
        }

        private void RoomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var roomTypeComboBox = sender as System.Windows.Controls.ComboBox;
            List<RoomType> types = new List<RoomType>();
            types.Add(RoomType.ExaminationRoom);
            types.Add(RoomType.OperatingRoom);
            types.Add(RoomType.RestRoom);
            roomTypeComboBox.ItemsSource = types;
            roomTypeComboBox.SelectedItem = room.type;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            string numberInput = numberBox.Text;
            int number = Int32.Parse(numberInput);

            int idx = roomRepository.rooms.FindIndex(room => room.number == number);
            if (roomRepository.rooms[idx]!=room)
            {  
                System.Windows.MessageBox.Show("This room number already exist!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RoomType type = (RoomType)typeComboBox.SelectedItem;
            if (type == null)
            {
                System.Windows.MessageBox.Show("Must select room type!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            room.number = number;
            room.type = type;
            System.Windows.MessageBox.Show("Room edited!", "Room edit", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
            RoomsTableWindow roomsTable = new RoomsTableWindow();
            roomsTable.ShowDialog();
        }
    }
}
