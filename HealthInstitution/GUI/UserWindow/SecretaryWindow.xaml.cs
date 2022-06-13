using HealthInstitution.GUI.LoginView;
using HealthInstitution.GUI.SecretaryView;
using HealthInstitution.GUI.SecretaryView.RequestsView;
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

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for SecretaryWindow.xaml
    /// </summary>
    public partial class SecretaryWindow : Window
    {
        public SecretaryWindow()
        {
            InitializeComponent();
        }

        private void Patients_Click(object sender, RoutedEventArgs e)
        {
            PatientsTable patientsTableWindow = new PatientsTable();
            patientsTableWindow.ShowDialog();
        }


        private void ExaminationRequests_Click(object sender, RoutedEventArgs e)
        {
            ExaminationRequestsReview examinationRequestsReview = new ExaminationRequestsReview();
            examinationRequestsReview.ShowDialog();
        }

        private void RestRequests_Click(object sender, RoutedEventArgs e)
        {
            RestRequestsReview restRequestsReview = new RestRequestsReview();
            restRequestsReview.ShowDialog();
        }

        private void UrgentExaminations_Click(object sender, RoutedEventArgs e)
        {
            AddUrgentExaminationDialog addUrgentExaminationDialog=new AddUrgentExaminationDialog();
            addUrgentExaminationDialog.ShowDialog();
        }

        private void UrgentOperations_Click(object sender, RoutedEventArgs e)
        {
            AddUrgentOperationDialog addUrgentOperationDialog = new AddUrgentOperationDialog();
            addUrgentOperationDialog.ShowDialog();
        }

        private void Scheduling_Click(object sender, RoutedEventArgs e)
        {
            PatientSelectionDialog patientSelectionDialog = new PatientSelectionDialog();
            patientSelectionDialog.ShowDialog();
        }
        private void DynamicEquipment_Click(object sender, RoutedEventArgs e)
        {
            DynamicEquipmentPurchaseDialog dynamicEquipmentPurchaseDialog = new DynamicEquipmentPurchaseDialog();
            dynamicEquipmentPurchaseDialog.ShowDialog();
        }

        private void TransferDynamicEquipment_Click(object sender, RoutedEventArgs e)
        {
            DynamicEquipmentReviewDialog dynamicEquipmentReviewDialog = new DynamicEquipmentReviewDialog();
            dynamicEquipmentReviewDialog.ShowDialog();
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

    }
}
