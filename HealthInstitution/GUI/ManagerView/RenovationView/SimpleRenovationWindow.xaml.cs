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
        private RoomRepository _roomRepository = RoomRepository.GetInstance();
        private ExaminationRepository _examinationRepository = ExaminationRepository.GetInstance();
        private OperationRepository _operationRepository = OperationRepository.GetInstance();
        private RenovationRepository _renovationRepository = RenovationRepository.GetInstance();
        public SimpleRenovationWindow()
        {
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
            List<Room> rooms = _roomRepository.GetActiveRooms();
            
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

            Room selectedRoom = (Room)roomComboBox.SelectedItem;
            if (selectedRoom.Type == RoomType.Warehouse)
            {
                System.Windows.MessageBox.Show("Warehouse cant be renovated!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckIfRoomHasScheduledExamination())
            {
                System.Windows.MessageBox.Show("Room has scheduled examination!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckIfRoomHasScheduledOperation())
            {
                System.Windows.MessageBox.Show("Room has scheduled operation!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckIfRoomHasScheduledRenovation())
            {
                System.Windows.MessageBox.Show("Room is already scheduled for renovation!", "Failed renovation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _renovationRepository.AddRenovation(selectedRoom, startDate, endDate);
            if (startDate == DateTime.Today)
            {
                RenovationChecker.StartRenovation(selectedRoom);
                System.Windows.MessageBox.Show("Renovation scheduled!", "Room renovation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                System.Windows.MessageBox.Show("Renovation scheduled!", "Room renovation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            this.Close();
        }

        private bool CheckIfRoomHasScheduledRenovation()
        {
            Room selectedRoom = (Room)roomComboBox.SelectedItem;
            foreach (Renovation renovation in _renovationRepository.GetAll())
            {
                if (renovation.Room == selectedRoom)
                {
                    return true;
                }
                if (renovation.GetType() == typeof(RoomMerger) && ((RoomMerger)renovation).RoomForMerge == selectedRoom)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckIfRoomHasScheduledOperation()
        {
            Room selectedRoom = (Room)roomComboBox.SelectedItem;
            
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
            Room selectedRoom = (Room)roomComboBox.SelectedItem;
           
            foreach (Examination examination in _examinationRepository.GetAll())
            {
                if (examination.Room == selectedRoom)
                {
                    return true;
                }                   
            }
            return false;
        }

        //private bool CheckIfDateIsInSpan(DateTime checkingDate, DateTime startDate, DateTime endDate)
        //{
        //    if (checkingDate >= startDate && checkingDate <= endDate)
        //        return true;
        //    return false;
        //}

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
