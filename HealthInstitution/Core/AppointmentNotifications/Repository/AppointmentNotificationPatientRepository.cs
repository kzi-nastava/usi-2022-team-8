using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Notifications.Repository
{
    public class AppointmentNotificationPatientRepository : IAppointmentNotificationPatientRepository
    {
        private String _fileName;
        private AppointmentNotificationPatientRepository(String fileName)
        {
            this._fileName = fileName;
            this.LoadFromFile();
        }

        private static AppointmentNotificationPatientRepository s_instance = null;

        public static AppointmentNotificationPatientRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new AppointmentNotificationPatientRepository(@"..\..\..\Data\JSON\appointmentNotificationPatient.json");
                }
                return s_instance;
            }
        }

        public void LoadFromFile()
        {
            var patientsByUsername = PatientRepository.GetInstance().PatientByUsername;
            var notificationsById = AppointmentNotificationRepository.GetInstance().NotificationsById;
            var patientUseranamesNotificationIds = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var pair in patientUseranamesNotificationIds)
            {
                int id = (int)pair["id"];
                String username = (String)pair["username"];
                Patient patient = patientsByUsername[username];
                AppointmentNotification notification = notificationsById[id];
                patient.Notifications.Add(notification);
                notification.Patient = patient;
            }
        }

        public void Save()
        {
            List<dynamic> patientUseranamesNotificationIds = new List<dynamic>();
            var notifications = AppointmentNotificationRepository.GetInstance().Notifications;
            foreach (var notification in notifications)
            {
                Patient patient=notification.Patient;
                patientUseranamesNotificationIds.Add(new { id = notification.Id, username = patient.Username });
            }
            var allPairs = JsonSerializer.Serialize(patientUseranamesNotificationIds);
            File.WriteAllText(this._fileName, allPairs);
        }
    }
}
