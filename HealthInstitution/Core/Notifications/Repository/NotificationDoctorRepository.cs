using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
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
    internal class NotificationDoctorRepository
    {
        private String _fileName;
        private NotificationDoctorRepository(String fileName)
        {
            this._fileName = fileName;
            this.LoadFromFile();
        }

        private static NotificationDoctorRepository s_instance = null;

        public static NotificationDoctorRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new NotificationDoctorRepository(@"..\..\..\Data\JSON\notificationDoctor.json");
                }
                return s_instance;
            }
        }

        public void LoadFromFile()
        {
            var doctorsByUsername = DoctorRepository.GetInstance().DoctorsByUsername;
            var notificationsById = NotificationRepository.GetInstance().NotificationsById;
            var doctorUseranamesNotificationIds = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var pair in doctorUseranamesNotificationIds)
            {
                int id = (int)pair["id"];
                String username = (String)pair["username"];
                Doctor doctor = doctorsByUsername[username];
                Notification notification = notificationsById[id];
                doctor.Notifications.Add(notification);
                notification.Doctor = doctor;
            }
        }

        public void Save()
        {
            List<dynamic> doctorUseranamesNotificationIds = new List<dynamic>();
            var notifications = NotificationRepository.GetInstance().Notifications;
            foreach (var notification in notifications)
            {
                Doctor doctor = notification.Doctor;
                doctorUseranamesNotificationIds.Add(new { id = notification.Id, username = doctor.Username });
            }
            var allPairs = JsonSerializer.Serialize(doctorUseranamesNotificationIds);
            File.WriteAllText(this._fileName, allPairs);
        }
    }
}
