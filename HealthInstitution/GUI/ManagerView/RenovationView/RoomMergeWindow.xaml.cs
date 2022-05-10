using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Renovations.Repository;
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
        private RenovationRepository _renovationRepository = RenovationRepository.GetInstance();
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
            List<Room> rooms = _roomRepository.GetActiveRooms();

            firstRoomComboBox.ItemsSource = rooms;
            firstRoomComboBox.SelectedItem = null;
        }

        private void SecondRoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<Room> rooms = _roomRepository.GetActiveRooms();

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

            DateTime startDate = (DateTime)startDatePicker.SelectedDate;
            DateTime endDate = (DateTime)endDatePicker.SelectedDate;

            if (startDate < DateTime.Today)
            {
                System.Windows.MessageBox.Show("You need to select future date!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (endDate < startDate)
            {
                System.Windows.MessageBox.Show("You need to select start date so that it is before end date!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string numberInput = numberBox.Text;

            if (numberInput.Trim() == "")
            {
                System.Windows.MessageBox.Show("Must input room number!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int number = Int32.Parse(numberInput);

            if (_roomRepository.Rooms.Any(room => room.Number == number))
            {
                System.Windows.MessageBox.Show("This room number already exist!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (number > 9999)
            {
                System.Windows.MessageBox.Show("This room number is too high!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Room firstSelectedRoom = (Room)firstRoomComboBox.SelectedItem;
            Room secondSelectedRoom = (Room)secondRoomComboBox.SelectedItem;
            if (firstSelectedRoom.Type == RoomType.Warehouse || secondSelectedRoom.Type == RoomType.Warehouse)
            {
                System.Windows.MessageBox.Show("Warehouse cant be renovated!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (firstSelectedRoom == secondSelectedRoom)
            {
                System.Windows.MessageBox.Show("You cant merge same room!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckIfRoomsHaveScheduledExamination())
            {
                System.Windows.MessageBox.Show("Room has scheduled examination!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckIfRoomsHaveScheduledOperation())
            {
                System.Windows.MessageBox.Show("Room has scheduled operation!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckIfRoomsHaveScheduledRenovation())
            {
                System.Windows.MessageBox.Show("Room is already scheduled for renovation!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            RoomType type = (RoomType)roomTypeComboBox.SelectedItem;
            Room mergedRoom = _roomRepository.AddRoom(type, number, true, false);
            _renovationRepository.AddRoomMerger(firstSelectedRoom,secondSelectedRoom, mergedRoom, startDate, endDate);
            if (startDate == DateTime.Today)
            {
                RenovationChecker.StartMerge(firstSelectedRoom, secondSelectedRoom, mergedRoom);
                System.Windows.MessageBox.Show("Renovation scheduled!", "Room renovation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Renovation scheduled!", "Room renovation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.Close();
        }

        private bool CheckIfRoomsHaveScheduledRenovation()
        {
            Room firstSelectedRoom = (Room)firstRoomComboBox.SelectedItem;
            Room secondSelectedRoom = (Room)secondRoomComboBox.SelectedItem;

            foreach (Renovation renovation in _renovationRepository.GetAll())
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
