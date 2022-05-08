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
        private RoomRepository _roomRepository = RoomRepository.GetInstance();
        public AddRoomDialog()
        {
            InitializeComponent();
        }

        private void roomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var roomTypeComboBox = sender as System.Windows.Controls.ComboBox;
            List<RoomType> types = new List<RoomType>();
            types.Add(RoomType.ExaminationRoom);
            types.Add(RoomType.OperatingRoom);
            types.Add(RoomType.RestRoom);
            roomTypeComboBox.ItemsSource = types;
            roomTypeComboBox.SelectedItem = null;
        }

        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            string numberInput = numberBox.Text;

            if (numberInput.Trim() == "")
            {
                System.Windows.MessageBox.Show("Must input room number!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int number = Int32.Parse(numberInput);

            if (_roomRepository.Rooms.Any(room => room.Number == number))
            {
                System.Windows.MessageBox.Show("This room number already exist!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (number>9999)
            {
                System.Windows.MessageBox.Show("This room number is too high!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (typeComboBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Must select room type!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RoomType type = (RoomType)typeComboBox.SelectedItem;
            _roomRepository.AddRoom(type, number);
            System.Windows.MessageBox.Show("Room added!", "Room creation", MessageBoxButton.OK, MessageBoxImage.Information);
            
            this.Close();
        }
    }
}
