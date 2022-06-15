// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.GUI.LoginView;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.DoctorRatings.Repository;
using HealthInstitution.Core.DoctorRatings;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.EquipmentTransfers.Functionality;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Ingredients;
using HealthInstitution.Core.Ingredients.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Polls;
using HealthInstitution.Core.Polls.Repository;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.PrescriptionNotifications.Repository;
using HealthInstitution.Core.Prescriptions.Repository;
using HealthInstitution.Core.Prescriptions;
using HealthInstitution.Core.Referrals;
using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.Renovations.Functionality;
using HealthInstitution.Core.Renovations;
using HealthInstitution.Core.Renovations.Repository;
using HealthInstitution.Core.RestRequestNotifications.Repository;
using HealthInstitution.Core.RestRequestNotifications;
using HealthInstitution.Core.RestRequests.Repository;
using HealthInstitution.Core.RestRequests;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.ScheduleEditRequests.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.Core.TrollCounters.Repository;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;

namespace HealthInstitution
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var services = new DIServiceCollection();

            //services.RegisterSingleton<Random>();
            //services.RegisterTransient<Random>();
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


           // services.RegisterTransient<IServiceInConstructorOfSomeService, ServiceInConstructorOfSomeService>();

            services.RegisterSingleton<LoginWindow>();


            services.BuildContainer();


            // var service1 = DIContainer.GetService<ISomeService>();
            // var service2 = DIContainer.GetService<ISomeService>();

            

            var loginWindow = DIContainer.GetService<LoginWindow>();
            
            //Console.WriteLine(service1.Method());
            //Console.WriteLine(service2.Method());

            await loginWindow.StartAsync();

        }
    }
}




















