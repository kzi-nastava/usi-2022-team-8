using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.GUI.UserWindow;
using HealthInstitution.GUI.DoctorView;
using System.Windows;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.TrollCounters.Repository;
using HealthInstitution.Core.EquipmentTransfers.Functionality;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.Renovations;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.DoctorRatings.Repository;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Ingredients.Repository;
using HealthInstitution.Core.Ingredients;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Polls.Repository;
using HealthInstitution.Core.Polls;
using HealthInstitution.Core.PrescriptionNotifications.Repository;
using HealthInstitution.Core.Prescriptions.Repository;
using HealthInstitution.Core.Prescriptions;
using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.Referrals;
using HealthInstitution.Core.Renovations.Repository;
using HealthInstitution.Core.RestRequestNotifications.Repository;
using HealthInstitution.Core.RestRequestNotifications;
using HealthInstitution.Core.RestRequests.Repository;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.ScheduleEditRequests.Repository;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core;
using System.Windows.Controls;
using HealthInstitution.ViewModels.GUIViewModels;

using HealthInstitution.GUI.ManagerView.DrugView;
using HealthInstitution.GUI.ManagerView.IngredientView;
using HealthInstitution.GUI.ManagerView;
using HealthInstitution.GUI.ManagerView.PollView;
using HealthInstitution.GUI.ManagerView.RenovationView;
using HealthInstitution.GUI.PatientView.Polls;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.GUI.PatientWindows;
using HealthInstitution.GUI.SecretaryView;
using HealthInstitution.GUI.SecretaryView.RequestsView;
using HealthInstitution.Core.Timetable;

namespace HealthInstitution.GUI.LoginView
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>

    public partial class LoginWindow : Window
    {
        private String _usernameInput;
        private String _passwordInput;

        private IUserService _userService;
        private ITrollCounterService _trollCounterService;
        private IPatientService _patientService;
        private IDoctorService _doctorService;
        private IPrescriptionNotificationService _prescriptionNotificationService;
        private IDoctorRatingsService _doctorRatingsService;

        public LoginWindow(IUserService userService, ITrollCounterService trollCounterService, IPatientService patientService, IDoctorService doctorService, IPrescriptionNotificationService prescriptionNotificationService, IDoctorRatingsService doctorRatingsService, IEquipmentTransferRefreshingService equipmentTransferRefreshingService, IRenovationRefreshingService renovationRefreshingService)
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel(this, userService, trollCounterService, patientService, doctorService, prescriptionNotificationService, doctorRatingsService);

            _userService = userService;
            _trollCounterService = trollCounterService;
            _patientService = patientService;
            _doctorService = doctorService;
            _prescriptionNotificationService = prescriptionNotificationService;
            _doctorRatingsService = doctorRatingsService;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword;
            }
        }

        [STAThread]
        private static void Main(string[] args)
        {
            var services = new DIServiceCollection();

            {
                services.RegisterSingleton<IAppointmentNotificationDoctorRepository, AppointmentNotificationDoctorRepository>();
                services.RegisterSingleton<IAppointmentNotificationPatientRepository, AppointmentNotificationPatientRepository>();
                services.RegisterSingleton<IAppointmentNotificationRepository, AppointmentNotificationRepository>();
                services.RegisterSingleton<IAppointmentNotificationService, DoctorRatingService>();

                services.RegisterSingleton<IDoctorRatingRepository, DoctorRatingRepository>();
                services.RegisterSingleton<IDoctorRatingsService, DoctorRatingsService>();

                services.RegisterSingleton<IDrugRepository, DrugRepository>();
                services.RegisterSingleton<IDrugService, DrugService>();
                services.RegisterSingleton<IDrugVerificationService, DrugVerificationService>();

                services.RegisterSingleton<IEquipmentRepository, EquipmentRepository>();
                services.RegisterSingleton<IEquipmentService, EquipmentService>();

                services.RegisterSingleton<IEquipmentTransferRepository, EquipmentTransferRepository>();
                services.RegisterSingleton<IEquipmentTransferService, EquipmentTransferService>();
                services.RegisterSingleton<IEquipmentTransferRefreshingService, EquipmentTransferRefreshingService>();

                services.RegisterSingleton<IExaminationRepository, ExaminationRepository>();
                services.RegisterSingleton<IExaminationDoctorRepository, ExaminationDoctorRepository>();
                services.RegisterSingleton<IExaminationService, ExaminationService>();

                services.RegisterSingleton<IIngredientRepository, IngredientRepository>();
                services.RegisterSingleton<IIngredientService, IngredientService>();

                services.RegisterSingleton<IMedicalRecordRepository, MedicalRecordRepository>();
                services.RegisterSingleton<IMedicalRecordService, MedicalRecordService>();

                services.RegisterSingleton<IOperationRepository, OperationRepository>();
                services.RegisterSingleton<IOperationDoctorRepository, OperationDoctorRepository>();
                services.RegisterSingleton<IOperationService, OperationService>();

                services.RegisterSingleton<IPollCommentRepository, PollCommentRepository>();
                services.RegisterSingleton<IPollQuestionRepository, PollQuestionRepository>();
                services.RegisterSingleton<IPollService, PollService>();

                services.RegisterSingleton<IPrescriptionNotificationRepository, PrescriptionNotificationRepository>();
                services.RegisterSingleton<IPrescriptionNotificationSettingsRepository, PrescriptionNotificationSettingsRepository>();
                services.RegisterSingleton<IPrescriptionNotificationService, PrescriptionNotificationService>();
                services.RegisterSingleton<IPrescriptionNotificationCronJobService, PrescriptionNotificationCronJobService>();

                services.RegisterSingleton<IPrescriptionRepository, PrescriptionRepository>();
                services.RegisterSingleton<IPrescriptionService, PrescriptionService>();

                services.RegisterSingleton<IReferralRepository, ReferralRepository>();
                services.RegisterSingleton<IReferralService, ReferralService>();

                services.RegisterSingleton<IRenovationRepository, RenovationRepository>();
                services.RegisterSingleton<IRenovationService, RenovationService>();
                services.RegisterSingleton<IRenovationRefreshingService, RenovationRefreshingService>();

                services.RegisterSingleton<IRestRequestNotificationRepository, RestRequestNotificationRepository>();
                services.RegisterSingleton<IRestRequestNotificationDoctorRepository, RestRequestNotificationDoctorRepository>();
                services.RegisterSingleton<IRestRequestNotificationService, RestRequestNotificationService>();

                services.RegisterSingleton<IRestRequestRepository, RestRequestRepository>();
                services.RegisterSingleton<IRestRequestDoctorRepository, RestRequestDoctorRepository>();
                services.RegisterSingleton<IRestRequestService, RestRequestService>();

                services.RegisterSingleton<IRoomRepository, RoomRepository>();
                services.RegisterSingleton<IRoomService, RoomService>();
                services.RegisterSingleton<IRoomTimetableService, RoomTimetableService>();

                services.RegisterSingleton<IScheduleEditRequestFileRepository, ScheduleEditRequestFileRepository>();
                services.RegisterSingleton<IScheduleEditRequestsService, ScheduleEditRequestService>();

                services.RegisterSingleton<IAppointmentDelayingService, AppointmentDelayingService>();
                services.RegisterSingleton<IDoctorExaminationAvailabilityService, DoctorExaminationAvailabilityService>();
                services.RegisterSingleton<IDoctorOperationAvailabilityService, DoctorOperationAvailabilityService>();
                services.RegisterSingleton<IEditSchedulingService, EditSchedulingService>();
                services.RegisterSingleton<IPatientExaminationAvailabilityService, PatientExaminationAvailabilityService>();
                services.RegisterSingleton<IPatientOperationAvailabilityService, PatientOperationAvailabilityService>();
                services.RegisterSingleton<IRecommendedSchedulingService, RecommendedSchedulingService>();
                services.RegisterSingleton<ISchedulingService, SchedulingService>();
                services.RegisterSingleton<IUrgentService, UrgentService>();

                services.RegisterSingleton<IDoctorRepository, DoctorRepository>();
                services.RegisterSingleton<IDoctorService, DoctorService>();

                services.RegisterSingleton<IPatientRepository, PatientRepository>();
                services.RegisterSingleton<IPatientService, PatientService>();

                services.RegisterSingleton<IUserRepository, UserRepository>();
                services.RegisterSingleton<IUserService, UserService>();

                services.RegisterSingleton<ITrollCounterFileRepository, TrollCounterFileRepository>();
                services.RegisterSingleton<ITrollCounterService, TrollCounterService>();

                services.RegisterSingleton<IDoctorTimetableService, DoctorTimetableService>();
            }

            //view
            {
                services.RegisterTransient<ManagerWindow>();
                services.RegisterTransient<DoctorWindow>();
                services.RegisterTransient<PatientWindow>();
                services.RegisterTransient<SecretaryWindow>();

                services.RegisterTransient<AddDrugDialog>();
                services.RegisterTransient<DrugsOnVerificationTableWindow>();
                services.RegisterTransient<EditDrugDialog>();
                services.RegisterTransient<RejectedDrugsTableWindow>();
                services.RegisterTransient<ReviseDrugDialog>();
                services.RegisterTransient<AddIngredientDialog>();
                services.RegisterTransient<EditIngredientDialog>();
                services.RegisterTransient<IngredientsTableWindow>();
                services.RegisterTransient<DoctorPollWindow>();
                services.RegisterTransient<HospitalPollWindow>();
                services.RegisterTransient<RatedDoctorsWindow>();
                services.RegisterTransient<ArrangeEquipmentForSplitWindow>();
                services.RegisterTransient<EquipmentTransferForSplitDialog>();
                services.RegisterTransient<RoomMergeWindow>();
                services.RegisterTransient<RoomSplitWindow>();
                services.RegisterTransient<SimpleRenovationWindow>();
                services.RegisterTransient<AddRoomDialog>();
                services.RegisterTransient<EditRoomDialog>();
                services.RegisterTransient<RoomsTableWindow>();
                services.RegisterTransient<EquipmentInspectionDialog>();
                services.RegisterTransient<EquipmentTableWindow>();
                services.RegisterTransient<EquipmentTransferDialog>();

                services.RegisterTransient<HealthInstitution.GUI.DoctorView.AddExaminationDialog>();
                services.RegisterTransient<AddOperationDialog>();
                services.RegisterTransient<AddPrescriptionDialog>();
                services.RegisterTransient<AddReferralDialog>();
                services.RegisterTransient<ConsumedEquipmentDialog>();
                services.RegisterTransient<DoctorNotificationsDialog>();
                services.RegisterTransient<DrugRejectionReasonDialog>();
                services.RegisterTransient<DrugsVerificationTable>();
                services.RegisterTransient<HealthInstitution.GUI.DoctorView.EditExaminationDialog>();
                services.RegisterTransient<EditOperationDialog>();
                services.RegisterTransient<ExaminationTable>();
                services.RegisterTransient<MedicalRecordDialog>();
                services.RegisterTransient<OperationTable>();
                services.RegisterTransient<PerformExaminationDialog>();
                services.RegisterTransient<ScheduledExaminationTable>();
                services.RegisterTransient<AddRestRequestDialog>();
                services.RegisterTransient<RestRequestTable>();

                services.RegisterTransient<DoctorPollDialog>();
                services.RegisterTransient<PatientHospitalPollDialog>();
                services.RegisterTransient<RecepieNotificationDialog>();
                services.RegisterTransient<RecepieNotificationSettingsDialog>();
                services.RegisterTransient<ClosestFit>();
                services.RegisterTransient<RecommendedWindow>();
                services.RegisterTransient<HealthInstitution.GUI.PatientView.AddExaminationDialog>();
                services.RegisterTransient<HealthInstitution.GUI.PatientWindows.EditExaminationDialog>();
                services.RegisterTransient<PatientScheduleWindow>();
                services.RegisterTransient<DoctorPickExamination>();
                services.RegisterTransient<MedicalRecordView>();
                services.RegisterTransient<PatientNotificationsDialog>();

                services.RegisterTransient<DynamicEquipmentPurchaseDialog>();
                services.RegisterTransient<DynamicEquipmentReviewDialog>();
                services.RegisterTransient<DynamicEquipmentTransferDialog>();
                services.RegisterTransient<CreatePatientDialog>();
                services.RegisterTransient<PatientsTable>();
                services.RegisterTransient<UpdatePatientWindow>();
                services.RegisterTransient<AddExaminationWithReferralDialog>();
                services.RegisterTransient<PatientReferralsDialog>();
                services.RegisterTransient<PatientSelectionDialog>();
                services.RegisterTransient<ExaminationRequestsReview>();
                services.RegisterTransient<RestRequestRejectionDialog>();
                services.RegisterTransient<RestRequestsReview>();
                services.RegisterTransient<AddUrgentExaminationDialog>();
                services.RegisterTransient<AddUrgentOperationDialog>();
                services.RegisterTransient<DelayExaminationOperationDialog>();
                services.RegisterTransient<UrgentExaminationDialog>();
                services.RegisterTransient<UrgentOperationDialog>();
            }

            services.RegisterTransient<LoginWindow>();

            services.BuildContainer();

            var loginWindow = DIContainer.GetService<LoginWindow>();
            loginWindow.ShowDialog();
        }
    }
}