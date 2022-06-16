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
        private String _fileName = @"..\..\..\Data\appointmentNotificationPatient.json";
        private IPatientRepository _patientRepository;
        private IAppointmentNotificationRepository _appointmentNotificationRepository;

        public AppointmentNotificationPatientRepository(IPatientRepository patientRepository, IAppointmentNotificationRepository appointmentNotificationRepository)
        {
            _patientRepository = patientRepository;
            _appointmentNotificationRepository = appointmentNotificationRepository;
            LoadFromFile();
        }

        public void LoadFromFile()
        {
            var patientsByUsername = _patientRepository.GetAllByUsername();
            var notificationsById = _appointmentNotificationRepository.GetAllById();
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
            var notifications = _appointmentNotificationRepository.GetAll();
            foreach (var notification in notifications)
            {
                Patient patient = notification.Patient;
                patientUseranamesNotificationIds.Add(new { id = notification.Id, username = patient.Username });
            }
            var allPairs = JsonSerializer.Serialize(patientUseranamesNotificationIds);
            File.WriteAllText(this._fileName, allPairs);
        }
    }
}