using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.RestRequestNotifications.Model;
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

namespace HealthInstitution.Core.RestRequestNotifications.Repository
{
    public class RestRequestNotificationDoctorRepository : IRestRequestNotificationDoctorRepository
    {
        private String _fileName = @"..\..\..\Data\restRequestNotificationDoctor.json";
        private IDoctorRepository _doctorRepository;
        private IRestRequestNotificationRepository _restRequestNotificationRepository;

        public RestRequestNotificationDoctorRepository(IDoctorRepository doctorRepository, IRestRequestNotificationRepository restRequestNotificationRepository)
        {
            _doctorRepository = doctorRepository;
            _restRequestNotificationRepository = restRequestNotificationRepository;
            this.LoadFromFile();
        }

        public void LoadFromFile()
        {
            var doctorsByUsername = _doctorRepository.GetAllByUsername();
            var notificationsById = _restRequestNotificationRepository.GetAllById();
            var doctorUseranamesNotificationIds = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var pair in doctorUseranamesNotificationIds)
            {
                int id = (int)pair["id"];
                String username = (String)pair["username"];
                Doctor doctor = doctorsByUsername[username];
                RestRequestNotification notification = notificationsById[id];
                doctor.RestRequestNotifications.Add(notification);
                notification.RestRequest.Doctor = doctor;
            }
        }

        public void Save()
        {
            List<dynamic> doctorUseranamesNotificationIds = new List<dynamic>();
            var notifications = _restRequestNotificationRepository.GetAll();
            foreach (var notification in notifications)
            {
                Doctor doctor = notification.RestRequest.Doctor;
                if (notification.Active)
                    doctorUseranamesNotificationIds.Add(new { id = notification.Id, username = doctor.Username });
            }
            var allPairs = JsonSerializer.Serialize(doctorUseranamesNotificationIds);
            File.WriteAllText(this._fileName, allPairs);
        }
    }
}