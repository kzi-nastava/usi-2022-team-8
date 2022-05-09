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

        private void StartMerge_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FirstRoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SecondRoomComboBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void RoomTypeComboBox_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
