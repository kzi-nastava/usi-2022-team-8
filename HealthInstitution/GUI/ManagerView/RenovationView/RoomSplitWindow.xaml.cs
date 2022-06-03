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
        private RoomRepository _roomRepository = RoomRepository.GetInstance();
        private ExaminationRepository _examinationRepository = ExaminationRepository.GetInstance();
        private OperationRepository _operationRepository = OperationRepository.GetInstance();
        private RenovationRepository _renovationRepository = RenovationRepository.GetInstance();
        private EquipmentRepository _equipmentRepository = EquipmentRepository.GetInstance();
        private EquipmentTransferRepository _equipmentTransferRepository = EquipmentTransferRepository.GetInstance();

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
            ClearFromList(_firstRoomEquipmentFromArranging);
            ClearFromList(_secondRoomEquipmentFromArranging);

            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;
            if (IsEmpty(selectedRoom.AvailableEquipment))
            {
                arrangeEquipmentButton.IsEnabled = false;
            }
            else
            {
                arrangeEquipmentButton.IsEnabled = true;
            }
        }

        private void ClearFromList(List<Equipment> equipments)
        {
            foreach(Equipment equipment in equipments)
            {
                EquipmentService.Delete(equipment.Id);
            }
            equipments.Clear();
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

            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;           
            if (!SplitRoomValidation(selectedRoom))
            {
                return;
            }

            ScheduleSeparation();
            
            this.Close();
        }

        private void ScheduleSeparation()
        {
            string firstNumberInput = firstRoomNumberBox.Text;
            string secondNumberInput = secondRoomNumberBox.Text;
            int firstRoomNumber = Int32.Parse(firstNumberInput);
            int secondRoomNumber = Int32.Parse(secondNumberInput);
            DateTime startDate = (DateTime)startDatePicker.SelectedDate;
            DateTime endDate = (DateTime)endDatePicker.SelectedDate;
            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;
            RoomType firstRoomType = (RoomType)firstRoomTypeComboBox.SelectedItem;
            RoomType secondRoomType = (RoomType)secondRoomTypeComboBox.SelectedItem;

            RoomDTO firstRoomDTO = new RoomDTO(firstRoomType, firstRoomNumber, true, false);
            Room firstRoom = RoomService.AddRoom(firstRoomDTO);
            RoomDTO secondRoomDTO = new RoomDTO(secondRoomType, secondRoomNumber, true, false);
            Room secondRoom = RoomService.AddRoom(secondRoomDTO);

            if (IsEmpty(_firstRoomEquipmentFromArranging) && IsEmpty(_secondRoomEquipmentFromArranging))
            {
                firstRoom.AvailableEquipment = CopyList(selectedRoom.AvailableEquipment);
                _roomRepository.Save();
            }
            else
            {
                firstRoom.AvailableEquipment = _firstRoomEquipmentFromArranging;
                secondRoom.AvailableEquipment = _secondRoomEquipmentFromArranging;
                _roomRepository.Save();
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

            if (_roomRepository.Rooms.Any(room => room.Number == firstRoomNumber || room.Number == secondRoomNumber))
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

        private bool SplitRoomValidation(Room selectedRoom)
        {
            if (selectedRoom.Type == RoomType.Warehouse)
            {
                System.Windows.MessageBox.Show("Warehouse cant be renovated!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (CheckIfRoomHasScheduledExamination())
            {
                System.Windows.MessageBox.Show("Room has scheduled examination!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (CheckIfRoomHasScheduledOperation())
            {
                System.Windows.MessageBox.Show("Room has scheduled operation!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (CheckIfRoomHasScheduledRenovation())
            {
                System.Windows.MessageBox.Show("Room is already scheduled for renovation!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (CheckIfRoomHasScheduledEquipmentTransfer())
            {
                System.Windows.MessageBox.Show("Room has equipment transfer for that room!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private List<Equipment> CopyList(List<Equipment> availableEquipment)
        {
            List<Equipment> equipments = new List<Equipment>();
            foreach (Equipment equipment in availableEquipment)
            {
                EquipmentDTO equipmentDTO = new EquipmentDTO(equipment.Quantity, equipment.Name, equipment.Type, equipment.IsDynamic);
                Equipment newEquipment = EquipmentService.Add(equipmentDTO);
                equipments.Add(newEquipment);
            }
            return equipments;
        }

        private bool CheckIfRoomHasScheduledEquipmentTransfer()
        {
            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;

            foreach (EquipmentTransfer equipmentTransfer in EquipmentTransferService.GetAll())
            {
                
                if (equipmentTransfer.FromRoom == selectedRoom || equipmentTransfer.ToRoom == selectedRoom)
                {
                    return true;
                }

            }
            return false;
        }
        private bool CheckIfRoomHasScheduledRenovation()
        {
            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;
            
            foreach (Renovation renovation in RenovationService.GetAll())
            {
                if (renovation.Room == selectedRoom)
                {
                    return true;
                }

                if (renovation.GetType() == typeof(RoomMerger))
                {
                    RoomMerger merger = (RoomMerger)renovation;
                    if (merger.RoomForMerge == selectedRoom)
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        private bool CheckIfRoomHasScheduledOperation()
        {
            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;
            
            foreach (Operation operation in _operationRepository.GetAll())
            {
                if (operation.Room == selectedRoom)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckIfRoomHasScheduledExamination()
        {
            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;
            
            foreach (Examination examination in _examinationRepository.GetAll())
            {
                if (examination.Room == selectedRoom)
                {
                    return true;
                }
            }
            return false;
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

        public static bool IsEmpty<T>(List<T> list)
        {
            if (list == null)
            {
                return true;
            }

            return !list.Any();
        }

        private void ArrangeEquipment_Click(object sender, RoutedEventArgs e)
        {
            Room selectedRoom = (Room)splitRoomComboBox.SelectedItem;

            if (!SplitRoomValidation(selectedRoom))
            {
                return;
            }

            if (IsEmpty(_firstRoomEquipmentFromArranging) && IsEmpty(_secondRoomEquipmentFromArranging))
            {
                _firstRoomEquipmentFromArranging = CopyList(selectedRoom.AvailableEquipment);
            }
            ArrangeEquipmentForSplitWindow arrangeEquipmentForSplitWindow = new ArrangeEquipmentForSplitWindow(_firstRoomEquipmentFromArranging,_secondRoomEquipmentFromArranging);
            arrangeEquipmentForSplitWindow.ShowDialog();
        }

        
    }
}
