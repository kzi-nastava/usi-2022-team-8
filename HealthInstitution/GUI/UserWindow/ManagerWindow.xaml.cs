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
                LoginWindow window = new LoginWindow(DIContainer.GetService<IUserService>(),
                                                    DIContainer.GetService<ITrollCounterService>(),
                                                    DIContainer.GetService<IPatientService>(),
                                                    DIContainer.GetService<IDoctorService>(),
                                                    DIContainer.GetService<IEquipmentTransferRefreshingService>(),
                                                    DIContainer.GetService<IRenovationRefreshingService>(),
                                                    DIContainer.GetService<IPrescriptionNotificationService>(),
                                                    DIContainer.GetService<IRestRequestService>(),
                                                    DIContainer.GetService<IDoctorRatingsService>());
                window.ShowDialog();
            }
        }

        private void EquipmentTransfer_Click(object sender, RoutedEventArgs e)
        {
            EquipmentTransferDialog equipmentTransferDialog = new EquipmentTransferDialog(DIContainer.GetService<IRenovationService>(), DIContainer.GetService<IEquipmentService>(),
                                                                                            DIContainer.GetService<IEquipmentTransferService>(), DIContainer.GetService<IRoomService>());
            equipmentTransferDialog.ShowDialog();
        }

        private void EquipmentInspection_Click(object sender, RoutedEventArgs e)
        {
            EquipmentInspectionDialog equipmentInspectionDialog = new EquipmentInspectionDialog(DIContainer.GetService<IEquipmentService>(), DIContainer.GetService<IRoomService>());
            equipmentInspectionDialog.ShowDialog();
        }

        private void Rooms_Click(object sender, RoutedEventArgs e)
        {
            RoomsTableWindow roomsTableWindow = new RoomsTableWindow(DIContainer.GetService<IRoomService>());
            roomsTableWindow.ShowDialog();
            
        }

        private void Renovate_Click(object sender, RoutedEventArgs e)
        {
            SimpleRenovationWindow simpleRenovationWindow = new SimpleRenovationWindow(DIContainer.GetService<IRoomService>(), DIContainer.GetService<IRoomTimetableService>(), DIContainer.GetService<IRenovationService>());
            simpleRenovationWindow.ShowDialog();
        }

        private void Ingredients_Click(object sender, RoutedEventArgs e)
        {
            IngredientsTableWindow ingredientsTableWindow = new IngredientsTableWindow(DIContainer.GetService<IIngredientService>());
            ingredientsTableWindow.ShowDialog();
        }

        private void OnVerificationDrugs_Click(object sender, RoutedEventArgs e)
        {
            DrugsOnVerificationTableWindow drugsOnVerificationTableWindow = new DrugsOnVerificationTableWindow(DIContainer.GetService<IDrugService>());
            drugsOnVerificationTableWindow.ShowDialog();
        }

        private void RejectedDrugs_Click(object sender, RoutedEventArgs e)
        {
            RejectedDrugsTableWindow rejectedDrugsTableWindow = new RejectedDrugsTableWindow(DIContainer.GetService<IDrugService>(), DIContainer.GetService<IDrugVerificationService>());
            rejectedDrugsTableWindow.ShowDialog();
        }

        private void HospitalPoll_Click(object sender, RoutedEventArgs e)
        {
            HospitalPollWindow hospitalPollWindow = new HospitalPollWindow(DIContainer.GetService<IPollService>());
            hospitalPollWindow.ShowDialog();
        }

        private void DoctorPoll_Click(object sender, RoutedEventArgs e)
        {
            DoctorPollWindow doctorPollWindow = new DoctorPollWindow(DIContainer.GetService<IDoctorRatingsService>(), DIContainer.GetService<IPollService>(), DIContainer.GetService<IDoctorService>());
            doctorPollWindow.ShowDialog();
        }
    }
}
