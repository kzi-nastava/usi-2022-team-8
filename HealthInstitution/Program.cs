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
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.GUI.UserWindow;
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

namespace HealthInstitution
{
    public class Program
    {
        public static void Start()
        {
        }
    }
}