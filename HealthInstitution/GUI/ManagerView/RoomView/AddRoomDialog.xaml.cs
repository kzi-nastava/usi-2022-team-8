﻿using HealthInstitution.Core.Rooms;
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
        IRoomService _roomService;
        public AddRoomDialog(IRoomService roomService)
        {
            _roomService = roomService;
            InitializeComponent();
        }

        private void RoomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<RoomType> types = new List<RoomType>();
            types.Add(RoomType.ExaminationRoom);
            types.Add(RoomType.OperatingRoom);
            types.Add(RoomType.RestRoom);
            typeComboBox.ItemsSource = types;
            typeComboBox.SelectedItem = null;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateRoomNumber())
            {
                return;
            }
            
            if (!ValidateRoomType())
            {
                return;
            }

            string numberInput = numberBox.Text;
            int number = Int32.Parse(numberInput);
            RoomType type = (RoomType)typeComboBox.SelectedItem;

            RoomDTO roomDTO = new RoomDTO(type, number);
            _roomService.AddRoom(roomDTO);
            System.Windows.MessageBox.Show("Room added!", "Room creation", MessageBoxButton.OK, MessageBoxImage.Information);
            
            this.Close();
        }

        private bool ValidateRoomType()
        {
            if (typeComboBox.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Must select room type!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private bool ValidateRoomNumber()
        {
            string numberInput = numberBox.Text;

            if (numberInput.Trim() == "")
            {
                System.Windows.MessageBox.Show("Must input room number!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            int number = Int32.Parse(numberInput);

            if (_roomService.RoomNumberIsTaken(number))
            {
                System.Windows.MessageBox.Show("This room number already exist!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (number > 9999)
            {
                System.Windows.MessageBox.Show("This room number is too high!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            
            return true;
        }
    }
}
