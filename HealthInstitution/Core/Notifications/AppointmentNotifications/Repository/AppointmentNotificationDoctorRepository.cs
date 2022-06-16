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
    public class AppointmentNotificationDoctorRepository : IAppointmentNotificationDoctorRepository
    {
        private String _fileName = @"..\..\..\Data\appointmentNotificationDoctor.json";
        private IDoctorRepository _doctorRepository;
        private IAppointmentNotificationRepository _appointmentNotificationRepository;

        public AppointmentNotificationDoctorRepository(IDoctorRepository doctorRepository, IAppointmentNotificationRepository appointmentNotificationRepository)
        {
            _doctorRepository = doctorRepository;
            _appointmentNotificationRepository = appointmentNotificationRepository;
            this.LoadFromFile();
        }

        public void LoadFromFile()
        {
            var doctorsByUsername = _doctorRepository.GetAllByUsername();
            var notificationsById = _appointmentNotificationRepository.GetAllById();

            var doctorUseranamesNotificationIds = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var pair in doctorUseranamesNotificationIds)
            {
                int id = (int)pair["id"];
                String username = (String)pair["username"];
                Doctor doctor = doctorsByUsername[username];
                AppointmentNotification notification = notificationsById[id];
                doctor.AppointmentNotifications.Add(notification);
                notification.Doctor = doctor;
            }
        }

        public void Save()
        {
            List<dynamic> doctorUseranamesNotificationIds = new List<dynamic>();
            var notifications = _appointmentNotificationRepository.GetAll();
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