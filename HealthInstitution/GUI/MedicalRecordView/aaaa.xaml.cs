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
using HealthInstitution.Core.SystemUsers.Users.Model;

namespace HealthInstitution.GUI.MedicalRecordView
{
    /// <summary>
    /// Interaction logic for aaaa.xaml
    /// </summary>
    public partial class aaaa : Window
    {
        public aaaa()
        {
            InitializeComponent();
            String abb = "s";
            User user = new User(UserType.Doctor, abb, abb, abb, abb);
            User user2 = new User(UserType.Doctor, "sss", abb, abb, abb);
            dataGrid.Items.Add(user);
            dataGrid.Items.Add(user);

        }

        /*[STAThread]
        static void Main(string[] args)
        {
            aaaa aaaa = new aaaa();
            aaaa.ShowDialog();
        }
*/
        private void dataGrid_CellClick(object sender, SelectedCellsChangedEventArgs e)
        {
            string ID = (dataGrid.SelectedItem as User).username;
            System.Windows.MessageBox.Show(ID);
        }


        private void addButton_click(object sender, RoutedEventArgs e)
        {

        }

        private void editButton_click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
