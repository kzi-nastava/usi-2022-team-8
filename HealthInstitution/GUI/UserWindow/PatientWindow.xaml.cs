using HealthInstitution.GUI.PatientWindows;
using System.Windows;

using HealthInstitution.GUI.LoginView;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Polls;

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        private Patient _loggedPatient;
        IPatientService _patientService;
        public PatientWindow(IPatientService patientService)
        {
            InitializeComponent();
            this._patientService = patientService;
            
            }
        public void SetLoggedPatient(Patient patient)
        {
            _loggedPatient = patient;
            ShowNotificationsDialog();
            RecepieNotificationDialog recepieNotificationDialog = DIContainer.GetService<RecepieNotificationDialog>();
            recepieNotificationDialog.SetLoggedPatient(_loggedPatient.Username);
            recepieNotificationDialog.ShowDialog();            
        }
        private void ShowNotificationsDialog()
        {
            if (_patientService.GetActiveAppointmentNotification(_loggedPatient).Count>0)
            {
                PatientNotificationsDialog patientNotificationsDialog = DIContainer.GetService<PatientNotificationsDialog>();
                patientNotificationsDialog.SetLoggedPatient(_loggedPatient);
                patientNotificationsDialog.ShowDialog();
            }
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

        private void ManuallSchedule_Click(object sender, RoutedEventArgs e)
        {
            PatientScheduleWindow patientScheduleWindow = DIContainer.GetService<PatientScheduleWindow>();
            patientScheduleWindow.SetLoggedPatient(_loggedPatient);
            patientScheduleWindow.ShowDialog();
        }

        private void RecommendedSchedule_Click(object sender, RoutedEventArgs e)
        {
            RecommendedWindow recommendedWindow = DIContainer.GetService<RecommendedWindow>();
            recommendedWindow.SetLoggedPatient(_loggedPatient);
            recommendedWindow.ShowDialog();           
        }

        private void MedicalRecordView_button_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecordView medicalRecordView = DIContainer.GetService<MedicalRecordView>();
            medicalRecordView.SetLoggedPatient(_loggedPatient);
            medicalRecordView.ShowDialog();       
        }

        private void PickDoctor_button_Click(object sender, RoutedEventArgs e)
        {
            DoctorPickExamination doctorPickExamination = DIContainer.GetService<DoctorPickExamination>();
            doctorPickExamination.SetLoggedPatient(_loggedPatient);
            doctorPickExamination.ShowDialog();            
        }

        private void RecepieNotificationSettings_button_Click(object sender, RoutedEventArgs e)
        {
            RecepieNotificationSettingsDialog recepieNotificationSettingsDialog = DIContainer.GetService<RecepieNotificationSettingsDialog>();
            recepieNotificationSettingsDialog.SetLoggedPatient(_loggedPatient.Username);
            recepieNotificationSettingsDialog.ShowDialog();            
        }

        private void rateHospital_button_Click(object sender, RoutedEventArgs e)
        {
            PatientHospitalPollDialog patientHospitalPollDialog = DIContainer.GetService<PatientHospitalPollDialog>();
            patientHospitalPollDialog.ShowDialog();           
        }
    }
}