using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
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
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HealthInstitution.GUI.ManagerView.RenovationView
{
    /// <summary>
    /// Interaction logic for RoomSplitWindow.xaml
    /// </summary>
    public partial class RoomSplitWindow : Window
    {
        private List<Equipment> _firstRoomEquipmentFromArranging;
        private List<Equipment> _secondRoomEquipmentFromArranging;
        public RoomSplitWindow()
        {
            InitializeComponent();
            _firstRoomEquipmentFromArranging = new List<Equipment>();
            _secondRoomEquipmentFromArranging = new List<Equipment>();
            arrangeEquipmentButton.IsEnabled = false;
        }

        private void SimpleRenovation_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            SimpleRenovationWindow simpleRenovationindow = new SimpleRenovationWindow();
            simpleRenovationindow.ShowDialog();
        }

        private void RoomMerge_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            RoomMergeWindow roomMergeWindow = new RoomMergeWindow();
            roomMergeWindow.ShowDialog();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SplittingRoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<Room> rooms = RoomService.GetActive();

            splitRoomComboBox.ItemsSource = rooms;
            splitRoomComboBox.SelectedItem = null;
        }

        private void FirstRoomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<RoomType> types = new List<RoomType>();
            types.Add(RoomType.ExaminationRoom);
            types.Add(RoomType.OperatingRoom);
            types.Add(RoomType.RestRoom);
            firstRoomTypeComboBox.ItemsSource = types;
            firstRoomTypeComboBox.SelectedItem = null;
        }

        private void SecondRoomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<RoomType> types = new List<RoomType>();
            types.Add(RoomType.ExaminationRoom);
            types.Add(RoomType.OperatingRoom);
            types.Add(RoomType.RestRoom);
            secondRoomTypeComboBox.ItemsSource = types;
            secondRoomTypeComboBox.SelectedItem = null;
        }

        private void SplitRoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EquipmentService.RemoveEquipmentFrom(_firstRoomEquipmentFromArranging);
            EquipmentService.RemoveEquipmentFrom(_secondRoomEquipmentFromArranging);

            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;
            if (EquipmentService.IsEmpty(selectedRoom.AvailableEquipment))
            {
                arrangeEquipmentButton.IsEnabled = false;
            }
            else
            {
                arrangeEquipmentButton.IsEnabled = true;
            }
        }

        private void StartSplit_Click(object sender, RoutedEventArgs e)
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

            if (!ValidateRoomNumbers())
            {
                return;
            }
           
            if (!SplitRoomValidation())
            {
                return;
            }

            ScheduleSeparation();
            
            this.Close();
        }
        private RoomDTO GetFirstRoom()
        {
            string firstNumberInput = firstRoomNumberBox.Text;
            int firstRoomNumber = Int32.Parse(firstNumberInput);
            RoomType firstRoomType = (RoomType)firstRoomTypeComboBox.SelectedItem;
            RoomDTO firstRoomDTO = new RoomDTO(firstRoomType, firstRoomNumber, true, false);
            return firstRoomDTO;
        }
        private RoomDTO GetSecondRoom()
        {
            string secondNumberInput = secondRoomNumberBox.Text;
            int secondRoomNumber = Int32.Parse(secondNumberInput);
            RoomType secondRoomType = (RoomType)secondRoomTypeComboBox.SelectedItem;
            RoomDTO secondRoomDTO = new RoomDTO(secondRoomType, secondRoomNumber, true, false);
            return secondRoomDTO;
        }
        private void ScheduleSeparation()
        {
            DateTime startDate = (DateTime)startDatePicker.SelectedDate;
            DateTime endDate = (DateTime)endDatePicker.SelectedDate;
            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;

            Room firstRoom = RoomService.AddRoom(GetFirstRoom());
            Room secondRoom = RoomService.AddRoom(GetSecondRoom());

            if (EquipmentService.IsEmpty(_firstRoomEquipmentFromArranging) && EquipmentService.IsEmpty(_secondRoomEquipmentFromArranging))
            {
                firstRoom.AvailableEquipment = EquipmentService.CopyEquipments(RoomService.GetAvailableEquipment(selectedRoom));
                RoomService.WriteIn();
            }
            else
            {
                firstRoom.AvailableEquipment = _firstRoomEquipmentFromArranging;
                secondRoom.AvailableEquipment = _secondRoomEquipmentFromArranging;
                RoomService.WriteIn();
            }

            RoomSeparationDTO roomSeparationDTO = new RoomSeparationDTO(selectedRoom, firstRoom, secondRoom, startDate, endDate);
            RenovationService.AddRoomSeparation(roomSeparationDTO);
            if (startDate == DateTime.Today)
            {
                RenovationService.StartSeparation(selectedRoom, firstRoom, secondRoom);
                System.Windows.MessageBox.Show("Renovation scheduled!", "Room renovation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Renovation scheduled!", "Room renovation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool ValidateRoomNumbers()
        {
            string firstNumberInput = firstRoomNumberBox.Text;
            string secondNumberInput = secondRoomNumberBox.Text;

            if (firstNumberInput.Trim() == "" || secondNumberInput.Trim() == "")
            {
                System.Windows.MessageBox.Show("Must input room number!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            int firstRoomNumber = Int32.Parse(firstNumberInput);
            int secondRoomNumber = Int32.Parse(secondNumberInput);

            if (firstRoomNumber == secondRoomNumber)
            {
                System.Windows.MessageBox.Show("Room numbers must be different!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (RoomService.RoomNumberIsTaken(firstRoomNumber) || RoomService.RoomNumberIsTaken(secondRoomNumber))
            {
                System.Windows.MessageBox.Show("This room number already exist!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (firstRoomNumber > 9999 || secondRoomNumber > 9999)
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

        private bool SplitRoomValidation()
        {
            DateTime startDate = (DateTime)startDatePicker.SelectedDate;
            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;

            if (selectedRoom.IsWarehouse())
            {
                System.Windows.MessageBox.Show("Warehouse cant be renovated!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string message;
            bool occupied = RoomTimetableService.CheckRoomTimetable(selectedRoom, startDate, out message);
            if (occupied)
            {
                System.Windows.MessageBox.Show(message, "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool CheckCompleteness()
        {
            if (splitRoomComboBox.SelectedItem == null)
            {
                return false;
            }
            if (firstRoomTypeComboBox.SelectedItem == null)
            {
                return false;
            }
            if (secondRoomTypeComboBox.SelectedItem == null)
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

        private void ArrangeEquipment_Click(object sender, RoutedEventArgs e)
        {
            
            if (!SplitRoomValidation())
            {
                return;
            }

            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;
            if (EquipmentService.IsEmpty(_firstRoomEquipmentFromArranging) && EquipmentService.IsEmpty(_secondRoomEquipmentFromArranging))
            {
                _firstRoomEquipmentFromArranging = EquipmentService.CopyEquipments(RoomService.GetAvailableEquipment(selectedRoom));
            }
            ArrangeEquipmentForSplitWindow arrangeEquipmentForSplitWindow = new ArrangeEquipmentForSplitWindow(_firstRoomEquipmentFromArranging,_secondRoomEquipmentFromArranging);
            arrangeEquipmentForSplitWindow.ShowDialog();
        }

        
    }
}
