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
    /// Interaction logic for SimpleRenovationWindow.xaml
    /// </summary>
    public partial class SimpleRenovationWindow : Window
    {     
        IRoomService _roomService;
        IRoomTimetableService _roomTimetableService;
        IRenovationService _renovationService;
        public SimpleRenovationWindow(IRoomService roomService, IRoomTimetableService roomTimetableService, IRenovationService renovationService)
        {
            _roomService = roomService;
            _roomTimetableService = roomTimetableService;
            _renovationService = renovationService;
            InitializeComponent();
        }

        private void RoomSplit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            RoomSplitWindow roomSplitWindow = new RoomSplitWindow();
            roomSplitWindow.ShowDialog();
        }

        private void RoomMerge_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            RoomMergeWindow roomMergeWindow = new RoomMergeWindow();
            roomMergeWindow.ShowDialog();
        }

        private void RoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {     
            List<Room> rooms = _roomService.GetActive();
            
            roomComboBox.ItemsSource = rooms;
            roomComboBox.SelectedItem = null;
        }

        private void StartRenovation_Click(object sender, RoutedEventArgs e)
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
            
            if (!ValidateSelectedRoom())
            {
                return;
            }

            ScheduleRenovation();
            
            this.Close();
        }

        private void ScheduleRenovation()
        {
            DateTime startDate = (DateTime)startDatePicker.SelectedDate;
            DateTime endDate = (DateTime)endDatePicker.SelectedDate;
            Room selectedRoom = (Room)roomComboBox.SelectedItem;

            RenovationDTO renovationDTO = new RenovationDTO(selectedRoom, startDate, endDate);
            _renovationService.AddRenovation(renovationDTO);
            if (startDate == DateTime.Today)
            {
                _renovationService.StartRenovation(selectedRoom);
                System.Windows.MessageBox.Show("Renovation scheduled!", "Room renovation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Renovation scheduled!", "Room renovation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool ValidateSelectedRoom()
        {
            Room selectedRoom = (Room)roomComboBox.SelectedItem;
            DateTime startDate = (DateTime)startDatePicker.SelectedDate;

            if (selectedRoom.IsWarehouse())
            {
                System.Windows.MessageBox.Show("Warehouse cant be renovated!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
      
            string message;
            bool occupied = _roomTimetableService.CheckRoomTimetable(selectedRoom, startDate, out message);
            if (occupied)
            {
                System.Windows.MessageBox.Show(message, "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (roomComboBox.SelectedItem == null)
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
