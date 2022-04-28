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
    /// Interaction logic for AddRoomDialog.xaml
    /// </summary>
    public partial class AddRoomDialog : Window
    {
        RoomRepository roomRepository = RoomRepository.GetInstance();
        public AddRoomDialog()
        {
            InitializeComponent();
        }

        private void RoomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var roomTypeComboBox = sender as System.Windows.Controls.ComboBox;
            List<RoomType> types = new List<RoomType>();
            types.Add(RoomType.ExaminationRoom);
            types.Add(RoomType.OperatingRoom);
            types.Add(RoomType.RestRoom);
            roomTypeComboBox.ItemsSource = types;
            roomTypeComboBox.SelectedIndex = 0;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string numberInput = numberBox.Text;
            int number = Int32.Parse(numberInput);

            if (roomRepository.rooms.Any(room => room.number == number))
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

            roomRepository.AddRoom(type, number);
            System.Windows.MessageBox.Show("Room added!", "Room creation", MessageBoxButton.OK, MessageBoxImage.Information);
            
            this.Close();
            RoomsTableWindow roomsTable = new RoomsTableWindow();
            roomsTable.ShowDialog();
        }
    }
}
