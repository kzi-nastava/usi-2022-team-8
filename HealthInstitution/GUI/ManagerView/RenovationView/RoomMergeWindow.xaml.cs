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
        private RoomRepository _roomRepository = RoomRepository.GetInstance();
        private ExaminationRepository _examinationRepository = ExaminationRepository.GetInstance();
        private OperationRepository _operationRepository = OperationRepository.GetInstance();
        
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
            if (firstSelectedRoom.Type == RoomType.Warehouse || secondSelectedRoom.Type == RoomType.Warehouse)
            {
                System.Windows.MessageBox.Show("Warehouse cant be renovated!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (firstSelectedRoom == secondSelectedRoom)
            {
                System.Windows.MessageBox.Show("You cant merge same room!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (CheckIfRoomsHaveScheduledExamination())
            {
                System.Windows.MessageBox.Show("Room has scheduled examination!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (CheckIfRoomsHaveScheduledOperation())
            {
                System.Windows.MessageBox.Show("Room has scheduled operation!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (CheckIfRoomsHaveScheduledRenovation())
            {
                System.Windows.MessageBox.Show("Room is already scheduled for renovation!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (CheckIfRoomsHaveScheduledEquipmentTransfer())
            {
                System.Windows.MessageBox.Show("Room has equipment transfer in that date span!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
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

            if (_roomRepository.Rooms.Any(room => room.Number == number))
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

        private bool CheckIfRoomsHaveScheduledEquipmentTransfer()
        {
            Room firstSelectedRoom = (Room)firstRoomComboBox.SelectedItem;
            Room secondSelectedRoom = (Room)secondRoomComboBox.SelectedItem;
            DateTime startDate = (DateTime)startDatePicker.SelectedDate;

            foreach (EquipmentTransfer equipmentTransfer in EquipmentTransferService.GetAll())
            {
                if (equipmentTransfer.TransferTime < startDate)
                {
                    continue;
                }
                if (equipmentTransfer.FromRoom == firstSelectedRoom || equipmentTransfer.ToRoom == firstSelectedRoom)
                {
                    return true;
                }
                if (equipmentTransfer.FromRoom == secondSelectedRoom || equipmentTransfer.ToRoom == secondSelectedRoom)
                {
                    return true;
                }

            }
            return false;
        }
        private bool CheckIfRoomsHaveScheduledRenovation()
        {
            Room firstSelectedRoom = (Room)firstRoomComboBox.SelectedItem;
            Room secondSelectedRoom = (Room)secondRoomComboBox.SelectedItem;

            foreach (Renovation renovation in RenovationService.GetAll())
            {
                if (renovation.Room == firstSelectedRoom || renovation.Room == secondSelectedRoom)
                {
                    return true;
                }

                if (renovation.GetType() == typeof(RoomMerger))
                {
                    RoomMerger merger = (RoomMerger)renovation;
                    if (merger.RoomForMerge == firstSelectedRoom || merger.RoomForMerge == secondSelectedRoom)
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }

        private bool CheckIfRoomsHaveScheduledOperation()
        {
            Room firstSelectedRoom = (Room)firstRoomComboBox.SelectedItem;
            Room secondSelectedRoom = (Room)secondRoomComboBox.SelectedItem;

            foreach (Operation operation in _operationRepository.GetAll())
            {
                if (operation.Room == firstSelectedRoom || operation.Room == secondSelectedRoom)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckIfRoomsHaveScheduledExamination()
        {
            Room firstSelectedRoom = (Room)firstRoomComboBox.SelectedItem;
            Room secondSelectedRoom = (Room)secondRoomComboBox.SelectedItem;
            foreach (Examination examination in _examinationRepository.GetAll())
            {
                if (examination.Room == firstSelectedRoom || examination.Room == secondSelectedRoom)
                {
                    return true;
                }
            }
            return false;
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
