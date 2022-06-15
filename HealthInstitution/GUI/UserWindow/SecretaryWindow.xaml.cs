using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.TrollCounters;
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
            PatientsTable patientsTableWindow = new PatientsTable(DIContainer.GetService<IPatientService>());
            patientsTableWindow.ShowDialog();
        }


        private void ExaminationRequests_Click(object sender, RoutedEventArgs e)
        {
            ExaminationRequestsReview examinationRequestsReview = new ExaminationRequestsReview(DIContainer.GetService<IScheduleEditRequestsService>());
            examinationRequestsReview.ShowDialog();
        }

        private void RestRequests_Click(object sender, RoutedEventArgs e)
        {
            RestRequestsReview restRequestsReview = new RestRequestsReview(DIContainer.GetService<IRestRequestService>());
            restRequestsReview.ShowDialog();
        }

        private void UrgentExaminations_Click(object sender, RoutedEventArgs e)
        {
            AddUrgentExaminationDialog addUrgentExaminationDialog=new AddUrgentExaminationDialog(DIContainer.GetService<IPatientService>(), DIContainer.GetService<IExaminationService>(), DIContainer.GetService<IAppointmentDelayingService>(), DIContainer.GetService<IMedicalRecordService>(), DIContainer.GetService<IUrgentService>());
            addUrgentExaminationDialog.ShowDialog();
        }

        private void UrgentOperations_Click(object sender, RoutedEventArgs e)
        {
            AddUrgentOperationDialog addUrgentOperationDialog = new AddUrgentOperationDialog(DIContainer.GetService<IPatientService>(), DIContainer.GetService<IOperationService>(), DIContainer.GetService<IAppointmentDelayingService>(), DIContainer.GetService<IMedicalRecordService>(), DIContainer.GetService<IUrgentService>());
            addUrgentOperationDialog.ShowDialog();
        }

        private void Scheduling_Click(object sender, RoutedEventArgs e)
        {
            PatientSelectionDialog patientSelectionDialog = new PatientSelectionDialog(DIContainer.GetService<IMedicalRecordService>());
            patientSelectionDialog.ShowDialog();
        }
        private void DynamicEquipment_Click(object sender, RoutedEventArgs e)
        {
            DynamicEquipmentPurchaseDialog dynamicEquipmentPurchaseDialog = new DynamicEquipmentPurchaseDialog(DIContainer.GetService<IEquipmentTransferService>(), DIContainer.GetService<IEquipmentService>());
            dynamicEquipmentPurchaseDialog.ShowDialog();
        }

        private void TransferDynamicEquipment_Click(object sender, RoutedEventArgs e)
        {
            DynamicEquipmentReviewDialog dynamicEquipmentReviewDialog = new DynamicEquipmentReviewDialog(DIContainer.GetService<IEquipmentService>());
            dynamicEquipmentReviewDialog.ShowDialog();
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

    }
}
