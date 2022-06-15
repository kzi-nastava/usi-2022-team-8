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
using HealthInstitution.GUI.ManagerView.PollView;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.Renovations;
using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Ingredients;
using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Polls;

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
                LoginWindow window = DIContainer.GetService<LoginWindow>();               
                window.ShowDialog();
            }
        }

        private void EquipmentTransfer_Click(object sender, RoutedEventArgs e)
        {
            EquipmentTransferDialog equipmentTransferDialog = DIContainer.GetService<EquipmentTransferDialog>();           
            equipmentTransferDialog.ShowDialog();
        }

        private void EquipmentInspection_Click(object sender, RoutedEventArgs e)
        {
            EquipmentInspectionDialog equipmentInspectionDialog = DIContainer.GetService<EquipmentInspectionDialog>();           
            equipmentInspectionDialog.ShowDialog();
        }

        private void Rooms_Click(object sender, RoutedEventArgs e)
        {
            RoomsTableWindow roomsTableWindow = DIContainer.GetService<RoomsTableWindow>();        
            roomsTableWindow.ShowDialog();
            
        }

        private void Renovate_Click(object sender, RoutedEventArgs e)
        {
            SimpleRenovationWindow simpleRenovationWindow = DIContainer.GetService<SimpleRenovationWindow>();
            simpleRenovationWindow.ShowDialog();
        }

        private void Ingredients_Click(object sender, RoutedEventArgs e)
        {
            IngredientsTableWindow ingredientsTableWindow = DIContainer.GetService<IngredientsTableWindow>();
            ingredientsTableWindow.ShowDialog();
        }

        private void OnVerificationDrugs_Click(object sender, RoutedEventArgs e)
        {
            DrugsOnVerificationTableWindow drugsOnVerificationTableWindow = DIContainer.GetService<DrugsOnVerificationTableWindow>();
            drugsOnVerificationTableWindow.ShowDialog();
        }

        private void RejectedDrugs_Click(object sender, RoutedEventArgs e)
        {
            RejectedDrugsTableWindow rejectedDrugsTableWindow = DIContainer.GetService<RejectedDrugsTableWindow>();
            rejectedDrugsTableWindow.ShowDialog();
        }

        private void HospitalPoll_Click(object sender, RoutedEventArgs e)
        {
            HospitalPollWindow hospitalPollWindow = DIContainer.GetService<HospitalPollWindow>();
            hospitalPollWindow.ShowDialog();
        }

        private void DoctorPoll_Click(object sender, RoutedEventArgs e)
        {
            DoctorPollWindow doctorPollWindow = DIContainer.GetService<DoctorPollWindow>();        
            doctorPollWindow.ShowDialog();
        }
    }
}
