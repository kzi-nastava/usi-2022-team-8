using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Renovations;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Renovations.Repository;
using HealthInstitution.Core.Rooms;
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

namespace HealthInstitution.GUI.ManagerView.RenovationView
{
    /// <summary>
    /// Interaction logic for RoomMergeWindow.xaml
    /// </summary>
    public partial class RoomMergeWindow : Window
    {
        public RoomMergeWindow()
        {
            InitializeComponent();
        }

        private void SimpleRenovation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            SimpleRenovationWindow simpleRenovationWindow = new SimpleRenovationWindow();
            simpleRenovationWindow.ShowDialog();
        }

        private void RoomSplit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            RoomSplitWindow roomSplitWindow = new RoomSplitWindow();
            roomSplitWindow.ShowDialog();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FirstRoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<Room> rooms = RoomService.GetActive();

            firstRoomComboBox.ItemsSource = rooms;
            firstRoomComboBox.SelectedItem = null;
        }

        private void SecondRoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<Room> rooms = RoomService.GetActive();

            secondRoomComboBox.ItemsSource = rooms;
            secondRoomComboBox.SelectedItem = null;
        }

        private void RoomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var roomTypeComboBox = sender as System.Windows.Controls.ComboBox;
            List<RoomType> types = new List<RoomType>();
            types.Add(RoomType.ExaminationRoom);
            types.Add(RoomType.OperatingRoom);
            types.Add(RoomType.RestRoom);
            roomTypeComboBox.ItemsSource = types;
            roomTypeComboBox.SelectedItem = null;
        }

        private void StartMerge_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckCompleteness())
            {
                System.Windows.MessageBox.Show("You need to select all data in form!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ValidateDate())
            {
                return;
            }           

            if (!ValidateRoomNumber())
            {
                return;
            }

            if (!ValidateSelectedRooms())
            {
                return;
            }
            
            ScheduleMerge();          
            this.Close();
        }

        private void ScheduleMerge()
        {
            DateTime startDate = (DateTime)startDatePicker.SelectedDate;
            DateTime endDate = (DateTime)endDatePicker.SelectedDate;
            string numberInput = numberBox.Text;
            int number = Int32.Parse(numberInput);
            Room firstSelectedRoom = (Room)firstRoomComboBox.SelectedItem;
            Room secondSelectedRoom = (Room)secondRoomComboBox.SelectedItem;
            RoomType type = (RoomType)roomTypeComboBox.SelectedItem;

            RoomDTO roomDTO = new RoomDTO(type, number, true, false);
            Room mergedRoom = RoomService.AddRoom(roomDTO);

            RoomMergerDTO roomMergerDTO = new RoomMergerDTO(firstSelectedRoom, secondSelectedRoom, mergedRoom, startDate, endDate);
            RenovationService.AddRoomMerger(roomMergerDTO);
            if (startDate == DateTime.Today)
            {
                RenovationService.StartMerge(firstSelectedRoom, secondSelectedRoom, mergedRoom);
                System.Windows.MessageBox.Show("Renovation scheduled!", "Room renovation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Renovation scheduled!", "Room renovation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool ValidateSelectedRooms()
        {
            Room firstSelectedRoom = (Room)firstRoomComboBox.SelectedItem;
            Room secondSelectedRoom = (Room)secondRoomComboBox.SelectedItem;
            DateTime startDate = (DateTime)startDatePicker.SelectedDate;

            if (firstSelectedRoom.IsWarehouse() || secondSelectedRoom.IsWarehouse())
            {
                System.Windows.MessageBox.Show("Warehouse cant be renovated!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (firstSelectedRoom == secondSelectedRoom)
            {
                System.Windows.MessageBox.Show("You cant merge same room!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string message;
            bool firstRoomOccupied = RoomTimetableService.CheckRoomTimetable(firstSelectedRoom, startDate, out message);
            if (firstRoomOccupied)
            {
                System.Windows.MessageBox.Show(message, "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            bool secondRoomOccupied = RoomTimetableService.CheckRoomTimetable(secondSelectedRoom, startDate, out message);
            if (secondRoomOccupied)
            {
                System.Windows.MessageBox.Show(message, "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool ValidateRoomNumber()
        {
            string numberInput = numberBox.Text;

            if (numberInput.Trim() == "")
            {
                System.Windows.MessageBox.Show("Must input room number!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            int number = Int32.Parse(numberInput);

            if (RoomService.RoomNumberIsTaken(number))
            {
                System.Windows.MessageBox.Show("This room number already exist!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (number > 9999)
            {
                System.Windows.MessageBox.Show("This room number is too high!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool ValidateDate()
        {
            DateTime startDate = (DateTime)startDatePicker.SelectedDate;
            DateTime endDate = (DateTime)endDatePicker.SelectedDate;

            if (startDate < DateTime.Today)
            {
                System.Windows.MessageBox.Show("You need to select future date!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (endDate < startDate)
            {
                System.Windows.MessageBox.Show("You need to select start date so that it is before end date!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool CheckCompleteness()
        {
            if (firstRoomComboBox.SelectedItem == null)
            {
                return false;
            }
            if (secondRoomComboBox.SelectedItem == null)
            {
                return false;
            }
            if (roomTypeComboBox.SelectedItem == null)
            {
                return false;
            }
            if (startDatePicker.SelectedDate == null)
            {
                return false;
            }
            if (endDatePicker.SelectedDate == null)
            {
                return false;
            }
            return true;
        }
    }
}
