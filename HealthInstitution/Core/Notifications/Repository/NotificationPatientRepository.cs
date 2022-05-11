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
    internal class NotificationPatientRepository
    {
        private String _fileName;
        private NotificationPatientRepository(String fileName)
        {
            this._fileName = fileName;
            this.LoadFromFile();
        }

        private static NotificationPatientRepository s_instance = null;

        public static NotificationPatientRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new NotificationPatientRepository(@"..\..\..\Data\JSON\notificationPatient.json");
                }
                return s_instance;
            }
        }

        public void LoadFromFile()
        {
            var patientsByUsername = PatientRepository.GetInstance().PatientByUsername;
            var notificationsById = NotificationRepository.GetInstance().NotificationsById;
            var patientUseranamesNotificationIds = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var pair in patientUseranamesNotificationIds)
            {
                int id = (int)pair["id"];
                String username = (String)pair["username"];
                Patient patient = patientsByUsername[username];
                Notification notification = notificationsById[id];
                patient.Notifications.Add(notification);
                notification.Patient = patient;
            }
        }

        public void Save()
        {
            List<dynamic> patientUseranamesNotificationIds = new List<dynamic>();
            var notifications = NotificationRepository.GetInstance().Notifications;
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
