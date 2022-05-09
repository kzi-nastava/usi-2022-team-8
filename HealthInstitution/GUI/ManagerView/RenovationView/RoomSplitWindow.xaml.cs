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
    /// Interaction logic for RoomSplitWindow.xaml
    /// </summary>
    public partial class RoomSplitWindow : Window
    {
        public RoomSplitWindow()
        {
            InitializeComponent();
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

        private void ArrangeEquipment_Click(object sender, RoutedEventArgs e)
        {
            ArrangeEquipmentForSplitWindow arrangeEquipmentForSplitWindow = new ArrangeEquipmentForSplitWindow();
            arrangeEquipmentForSplitWindow.ShowDialog();
        }

        private void StartSplit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SplittingRoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void FirstRoomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SecondRoomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
