using HealthInstitution.Core.RestRequestNotifications.Model;
using HealthInstitution.Core.RestRequests.Model;
using HealthInstitution.Core.RestRequests.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RestRequestNotifications.Repository
{
    public class RestRequestNotificationRepository : IRestRequestNotificationRepository
    {
        private String _fileName = @"..\..\..\Data\restRequestNotifications.json";
        public int _maxId { get; set; }
        public List<RestRequestNotification> Notifications { get; set; }
        public Dictionary<int, RestRequestNotification> NotificationsById { get; set; }
        private IRestRequestRepository _restRequestRepository;

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        public RestRequestNotificationRepository(IRestRequestRepository restRequestRepository)
        {
            _restRequestRepository = restRequestRepository;
            this.Notifications = new List<RestRequestNotification>();
            this.NotificationsById = new Dictionary<int, RestRequestNotification>();
            this._maxId = 0;
            this.LoadFromFile();
        }

        public void LoadFromFile()
        {
            var allNotifications = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var notification in allNotifications)
            {
                int id = (int)notification["id"];
                int restRequestId = (int)notification["restRequestId"];
                RestRequest restRequest = _restRequestRepository.GetById(restRequestId);
                bool active = (bool)notification["active"];

                RestRequestNotification loadedNotification = new RestRequestNotification(id, restRequest, active);

                if (id > _maxId) { _maxId = id; }

                this.Notifications.Add(loadedNotification);
                this.NotificationsById.Add(id, loadedNotification);
            }
        }

        public void Save()
        {
            List<dynamic> reducedNotifications = new List<dynamic>();
            foreach (RestRequestNotification notification in this.Notifications)
            {
                reducedNotifications.Add(new
                {
                    id = notification.Id,
                    restRequestId = notification.RestRequest.Id,
                    active = notification.Active
                });
            }
            var allNotifications = JsonSerializer.Serialize(reducedNotifications, _options);
            File.WriteAllText(this._fileName, allNotifications);
        }

        public List<RestRequestNotification> GetAll()
        {
            return this.Notifications;
        }

        public Dictionary<int, RestRequestNotification> GetAllById()
        {
            return NotificationsById;
        }

        public RestRequestNotification GetById(int id)
        {
            if (NotificationsById.ContainsKey(id))
            {
                return NotificationsById[id];
            }
            return null;
        }

        private void AddToCollections(RestRequestNotification notification)
        {
            notification.RestRequest.Doctor.RestRequestNotifications.Add(notification);
            Notifications.Add(notification);
            NotificationsById.Add(notification.Id, notification);
        }

        private void SaveAll()
        {
            Save();
            DIContainer.DIContainer.GetService<IRestRequestNotificationDoctorRepository>().Save();
        }

        public void Add(RestRequestNotification restRequestNotification)
        {
            int id = ++this._maxId;
            restRequestNotification.Id = id;
            AddToCollections(restRequestNotification);
            SaveAll();
        }
    }
}