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

namespace HealthInstitution.GUI.LoginView
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>

    public partial class LoginWindow : Window
    {
        private String _usernameInput;
        private String _passwordInput;

        IUserService _userService;
        ITrollCounterService _trollCounterService;
        IPatientService _patientService;
        IDoctorService _doctorService;
        IEquipmentTransferRefreshingService _equipmentTransferRefreshingService;
        IRenovationRefreshingService _renovationRefreshingService;
        IPrescriptionNotificationService _prescriptionNotificationService;
        IRestRequestService _restRequestService;
        IDoctorRatingsService _doctorRatingsService;


        public LoginWindow(IUserService userService, ITrollCounterService trollCounterService, IPatientService patientService, IDoctorService doctorService, IEquipmentTransferRefreshingService equipmentTransferRefreshingService, IRenovationRefreshingService renovationRefreshingService, IPrescriptionNotificationService prescriptionNotificationService, IRestRequestService restRequestService, IDoctorRatingsService doctorRatingsService)
        {
            InitializeComponent();
            _userService = userService;
            _trollCounterService = trollCounterService;
            _patientService = patientService;
            _doctorService = doctorService;
            _equipmentTransferRefreshingService = equipmentTransferRefreshingService;
            _renovationRefreshingService = renovationRefreshingService;
            _prescriptionNotificationService = prescriptionNotificationService;
            _restRequestService = restRequestService;
            _doctorRatingsService = doctorRatingsService;
        }

        private User GetUserFromInputData()
        {
            _usernameInput = usernameBox.Text;
            _passwordInput = passwordBox.Password.ToString();
            return _userService.GetByUsername(_usernameInput);
        }

        private void LoginButton_click(object sender, RoutedEventArgs e)
        {
            User user = GetUserFromInputData();
            if (_userService.IsUserFound(user, _passwordInput) && !_userService.IsUserBlocked(user))
            {
                this.Close();
                switch (user.Type)
                {
                    case UserType.Patient:
                        RedirectPatient(user);
                        break;

                    case UserType.Doctor:
                        RedirectDoctor();
                        break;

                    case UserType.Secretary:
                        RedirectSecretary();
                        break;

                    case UserType.Manager:
                        RedirectManager();
                        break;
                }
            }
        }

        private void RedirectPatient(User foundUser)
        {
            _trollCounterService.TrollCheck(foundUser.Username);
            _patientService.LoadNotifications();
            Patient loggedPatient = _patientService.GetByUsername(_usernameInput);
            _prescriptionNotificationService.GenerateAllSkippedNotifications(loggedPatient.Username);
            _doctorRatingsService.AssignScores();

            var patientWindow = DIContainer.GetService<PatientWindow>();
            patientWindow.SetLoggedPatient(loggedPatient);
            patientWindow.ShowDialog();
            
        }

        private void RedirectDoctor()
        {
            _doctorService.LoadAppointments();
            _doctorService.LoadNotifications();
            Doctor loggedDoctor = _doctorService.GetById(_usernameInput);

            var doctorWindow = DIContainer.GetService<DoctorWindow>();
            doctorWindow.SetLoggedDoctor(loggedDoctor);
            doctorWindow.ShowDialog();
        }

        private void RedirectSecretary()
        {
            _restRequestService.LoadRequests();

            var secretaryWindow = DIContainer.GetService<SecretaryWindow>();
            secretaryWindow.ShowDialog();

        }

        private void RedirectManager()
        {
            var managerWindow = DIContainer.GetService<ManagerWindow>();
            managerWindow.ShowDialog();

        }

        [STAThread]
        static void Main(string[] args)
        {
            var services = new DIServiceCollection();

            {
                services.RegisterTransient<IAppointmentNotificationDoctorRepository, AppointmentNotificationDoctorRepository>();
                services.RegisterTransient<IAppointmentNotificationPatientRepository, AppointmentNotificationPatientRepository>();
                services.RegisterTransient<IAppointmentNotificationRepository, AppointmentNotificationRepository>();
                services.RegisterTransient<IAppointmentNotificationService, DoctorRatingService>();

                services.RegisterTransient<IDoctorRatingRepository, DoctorRatingRepository>();
                services.RegisterTransient<IDoctorRatingsService, DoctorRatingsService>();

                services.RegisterTransient<IDrugRepository, DrugRepository>();
                services.RegisterTransient<IDrugService, DrugService>();
                services.RegisterTransient<IDrugVerificationService, DrugVerificationService>();

                services.RegisterTransient<IEquipmentRepository, EquipmentRepository>();
                services.RegisterTransient<IEquipmentService, EquipmentService>();

                services.RegisterTransient<IEquipmentTransferRepository, EquipmentTransferRepository>();
                services.RegisterTransient<IEquipmentTransferService, EquipmentTransferService>();
                services.RegisterTransient<IEquipmentTransferRefreshingService, EquipmentTransferRefreshingService>();

                services.RegisterTransient<IExaminationRepository, ExaminationRepository>();
                services.RegisterTransient<IExaminationDoctorRepository, ExaminationDoctorRepository>();
                services.RegisterTransient<IExaminationService, ExaminationService>();

                services.RegisterTransient<IIngredientRepository, IngredientRepository>();
                services.RegisterTransient<IIngredientService, IngredientService>();

                services.RegisterTransient<IMedicalRecordRepository, MedicalRecordRepository>();
                services.RegisterTransient<IMedicalRecordService, MedicalRecordService>();

                services.RegisterTransient<IOperationRepository, OperationRepository>();
                services.RegisterTransient<IOperationDoctorRepository, OperationDoctorRepository>();
                services.RegisterTransient<IOperationService, OperationService>();

                services.RegisterTransient<IPollCommentRepository, PollCommentRepository>();
                services.RegisterTransient<IPollQuestionRepository, PollQuestionRepository>();
                services.RegisterTransient<IPollService, PollService>();

                services.RegisterTransient<IPrescriptionNotificationRepository, PrescriptionNotificationRepository>();
                services.RegisterTransient<IPrescriptionNotificationSettingsRepository, PrescriptionNotificationSettingsRepository>();
                services.RegisterTransient<IPrescriptionNotificationService, PrescriptionNotificationService>();
                services.RegisterTransient<IPrescriptionNotificationCronJobService, PrescriptionNotificationCronJobService>();

                services.RegisterTransient<IPrescriptionRepository, PrescriptionRepository>();
                services.RegisterTransient<IPrescriptionService, PrescriptionService>();

                services.RegisterTransient<IReferralRepository, ReferralRepository>();
                services.RegisterTransient<IReferralService, ReferralService>();

                services.RegisterTransient<IRenovationRepository, RenovationRepository>();
                services.RegisterTransient<IRenovationService, RenovationService>();
                services.RegisterTransient<IRenovationRefreshingService, RenovationRefreshingService>();

                services.RegisterTransient<IRestRequestNotificationRepository, RestRequestNotificationRepository>();
                services.RegisterTransient<IRestRequestNotificationDoctorRepository, RestRequestNotificationDoctorRepository>();
                services.RegisterTransient<IRestRequestNotificationService, RestRequestNotificationService>();

                services.RegisterTransient<IRestRequestRepository, RestRequestRepository>();
                services.RegisterTransient<IRestRequestDoctorRepository, RestRequestDoctorRepository>();
                services.RegisterTransient<IRestRequestService, RestRequestService>();

                services.RegisterTransient<IRoomRepository, RoomRepository>();
                services.RegisterTransient<IRoomService, RoomService>();
                services.RegisterTransient<IRoomTimetableService, RoomTimetableService>();

                services.RegisterTransient<IScheduleEditRequestFileRepository, ScheduleEditRequestFileRepository>();
                services.RegisterTransient<IScheduleEditRequestsService, ScheduleEditRequestService>();

                services.RegisterTransient<IAppointmentDelayingService, AppointmentDelayingService>();
                services.RegisterTransient<IDoctorExaminationAvailabilityService, DoctorExaminationAvailabilityService>();
                services.RegisterTransient<IDoctorOperationAvailabilityService, DoctorOperationAvailabilityService>();
                services.RegisterTransient<IEditSchedulingService, EditSchedulingService>();
                services.RegisterTransient<IPatientExaminationAvailabilityService, PatientExaminationAvailabilityService>();
                services.RegisterTransient<IPatientOperationAvailabilityService, PatientOperationAvailabilityService>();
                services.RegisterTransient<IRecommendedSchedulingService, RecommendedSchedulingService>();
                services.RegisterTransient<ISchedulingService, SchedulingService>();
                services.RegisterTransient<IUrgentService, UrgentService>();

                services.RegisterTransient<IDoctorRepository, DoctorRepository>();
                services.RegisterTransient<IDoctorService, DoctorService>();

                services.RegisterTransient<IPatientRepository, PatientRepository>();
                services.RegisterTransient<IPatientService, PatientService>();

                services.RegisterTransient<IUserRepository, UserRepository>();
                services.RegisterTransient<IUserService, UserService>();

                services.RegisterTransient<ITrollCounterFileRepository, TrollCounterFileRepository>();
                services.RegisterTransient<ITrollCounterService, TrollCounterService>();

                services.RegisterTransient<ITimetableService, TimetableService>();
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


            //var loginWindow = DIContainer.GetService<LoginWindow>();

            //DIContainer.GetService<IEquipmentTransferRefreshingService>().UpdateByTransfer();
            //DIContainer.GetService<IRenovationRefreshingService>().UpdateByRenovation();

            //_equipmentTransferRefreshingService.UpdateByTransfer();
            //_renovationRefreshingService.UpdateByRenovation();

            var loginWindow = DIContainer.GetService<LoginWindow>();
            loginWindow.ShowDialog();

        }
    }
}