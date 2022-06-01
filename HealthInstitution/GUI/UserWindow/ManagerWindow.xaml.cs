using HealthInstitution.GUI.ManagerView;
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
using HealthInstitution.GUI.LoginView;
using HealthInstitution.GUI.ManagerView.RenovationView;
using HealthInstitution.GUI.ManagerView.DrugView;

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to log out?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                this.Close();
                LoginWindow window = new LoginWindow();
                window.ShowDialog();
            }
        }

        private void EquipmentTransfer_Click(object sender, RoutedEventArgs e)
        {
            EquipmentTransferDialog equipmentTransferDialog = new EquipmentTransferDialog();
            equipmentTransferDialog.ShowDialog();
        }

        private void EquipmentInspection_Click(object sender, RoutedEventArgs e)
        {
            EquipmentInspectionDialog equipmentInspectionDialog = new EquipmentInspectionDialog();
            equipmentInspectionDialog.ShowDialog();
        }

        private void Rooms_Click(object sender, RoutedEventArgs e)
        {
            RoomsTableWindow roomsTableWindow = new RoomsTableWindow();
            roomsTableWindow.ShowDialog();
            
        }

        private void Renovate_Click(object sender, RoutedEventArgs e)
        {
            SimpleRenovationWindow simpleRenovationWindow = new SimpleRenovationWindow();
            simpleRenovationWindow.ShowDialog();
        }

        private void Ingredients_Click(object sender, RoutedEventArgs e)
        {
            IngredientsTableWindow ingredientsTableWindow = new IngredientsTableWindow();
            ingredientsTableWindow.ShowDialog();
        }

        private void OnVerificationDrugs_Click(object sender, RoutedEventArgs e)
        {
            DrugsOnVerificationTableWindow drugsOnVerificationTableWindow = new DrugsOnVerificationTableWindow();
            drugsOnVerificationTableWindow.ShowDialog();
        }

        private void RejectedDrugs_Click(object sender, RoutedEventArgs e)
        {
            RejectedDrugsTableWindow rejectedDrugsTableWindow = new RejectedDrugsTableWindow();
            rejectedDrugsTableWindow.ShowDialog();
        }
    }
}
