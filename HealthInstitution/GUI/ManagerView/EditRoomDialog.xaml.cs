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
        private Room _room;
        private RoomRepository _roomRepository = RoomRepository.GetInstance();
        public EditRoomDialog(Room room)
        {
            InitializeComponent();
            this._room = room;
            setRoomData();
        }

        private void setRoomData()
        {
            numberBox.Text = _room.Number.ToString();
        }

        private void roomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var roomTypeComboBox = sender as System.Windows.Controls.ComboBox;
            List<RoomType> types = new List<RoomType>();
            types.Add(RoomType.ExaminationRoom);
            types.Add(RoomType.OperatingRoom);
            types.Add(RoomType.RestRoom);
            roomTypeComboBox.ItemsSource = types;
            roomTypeComboBox.SelectedItem = _room.Type;
        }

        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            string numberInput = numberBox.Text;

            if (numberInput.Trim() == "")
            {
                System.Windows.MessageBox.Show("Must input room number!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int number = Int32.Parse(numberInput);

            int idx = _roomRepository.Rooms.FindIndex(room => room.Number == number);
            if (idx >= 0)
            {
                if (_roomRepository.Rooms[idx] != _room)
                {
                    System.Windows.MessageBox.Show("This room number already exist!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (typeComboBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Must select room type!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RoomType type = (RoomType)typeComboBox.SelectedItem;

            _roomRepository.Update(_room.Id, type, number, _room.IsRenovating);
            System.Windows.MessageBox.Show("Room edited!", "Room edit", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }
    }
}
