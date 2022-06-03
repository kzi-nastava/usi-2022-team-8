using HealthInstitution.Core.RecepieNotifications.Model;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.RecepieNotifications.Repository;

public class RecepieNotificationRepository
{
    private String _fileName;
    public List<RecepieNotification> Notifications { get; set; }
    public Dictionary<int, RecepieNotification> NotificationsById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    private RecepieNotificationRepository(String fileName)
    {
        this._fileName = fileName;
        this.Notifications = new List<RecepieNotification>();
        this.NotificationsById = new Dictionary<int, RecepieNotification>();
        this.LoadFromFile();
    }

    private static RecepieNotificationRepository s_instance = null;

    public static RecepieNotificationRepository GetInstance()
    {
        {
            if (s_instance == null)
            {
                s_instance = new RecepieNotificationRepository(@"..\..\..\Data\JSON\recepieNotifications.json");
            }
            return s_instance;
        }
    }

    public void LoadFromFile()
    {
        var notifications = JsonSerializer.Deserialize<List<RecepieNotification>>(File.ReadAllText(@"..\..\..\Data\JSON\recepieNotifications.json"), _options);
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

    public RecepieNotification GetById(int id)
    {
        return this.NotificationsById[id];
    }

    public void Add(RecepieNotification recepieNotification)
    {
        this.Notifications.Add(recepieNotification);
        this.NotificationsById.Add(recepieNotification.Id, recepieNotification);
        Save();
    }

    public void Delete(int id)
    {
        RecepieNotification notification = NotificationsById[id];
        if (notification != null)
        {
            this.NotificationsById.Remove(notification.Id);
            this.Notifications.Remove(notification);
            Save();
        }
    }

    public List<RecepieNotification> GetPatientPresctiptionNotification(string username, int prescription)
    {
        List<RecepieNotification> ownNotifications = new List<RecepieNotification>();
        foreach (var notification in this.Notifications)
        {
            if (notification.Patient == username && notification.Prescription.Id == prescription) ownNotifications.Add(notification);
        }
        return ownNotifications;
    }

    public List<RecepieNotification> GetPatientActiveNotification(string username)
    {
        List<RecepieNotification> ownNotifications = new List<RecepieNotification>();
        foreach (var notification in this.Notifications)
        {
            if (notification.Patient == username && notification.ActiveForPatient)
            {
                ownNotifications.Add(notification);
                notification.ActiveForPatient = false;
            }
        }
        Save();
        return ownNotifications;
    }
}