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

        }

        private void StartRenovation_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
