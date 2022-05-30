using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using HealthInstitution.Core.RecepieNotifications.Model;

namespace HealthInstitution.Core.RecepieNotifications.Repository;

public class RecepieNotificationRepository
{
    private String _fileName;
    public List<RecepieNotification> Notifications { get; set; }
    public Dictionary<String, RecepieNotification> NotificationsById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    private RecepieNotificationRepository(String fileName)
    {
        this._fileName = fileName;
        this.Notifications = new List<RecepieNotification>();
        this.NotificationsById = new Dictionary<String, RecepieNotification>();
        this.LoadFromFile();
    }

    private static RecepieNotificationRepository s_instance = null;

    public static RecepieNotificationRepository GetInstance()
    {
        {
            if (s_instance == null)
            {
                s_instance = new RecepieNotificationRepository(@"..\..\..\Data\JSON\recepieNotification.json");
            }
            return s_instance;
        }
    }

    public void LoadFromFile()
    {
        var notifications = JsonSerializer.Deserialize<List<RecepieNotification>>(File.ReadAllText(@"..\..\..\Data\JSON\recepieNotification.json"), _options);
        foreach (RecepieNotification notification in notifications)
        {
            this.Notifications.Add(notification);
            this.NotificationsById.Add(notification.Id, notification);
        }
    }

    public void Save()
    {
        var allRatings = JsonSerializer.Serialize(this.Notifications, _options);
        File.WriteAllText(this._fileName, allRatings);
    }

    public RecepieNotification GetById(string id)
    {
        return this.NotificationsById[id];
    }

    public void Add(RecepieNotification recepieNotification)
    {
        this.Notifications.Add(recepieNotification);
        this.NotificationsById.Add(recepieNotification.Id, recepieNotification);
        Save();
    }

    public void Delete(string id)
    {
        RecepieNotification notification = NotificationsById[id];
        if (notification != null)
        {
            this.NotificationsById.Remove(notification.Id);
            this.Notifications.Remove(notification);
            Save();
        }
    }

    public List<RecepieNotification> GetPatientPresctiptionNotification(string username, string prescription)
    {
        List<RecepieNotification> ownNotifications = new List<RecepieNotification>();
        foreach (var notification in this.Notifications)
        {
            if (notification.Patient == username && notification.Id == prescription) ownNotifications.Add(notification);
        }
        return ownNotifications;
    }
}