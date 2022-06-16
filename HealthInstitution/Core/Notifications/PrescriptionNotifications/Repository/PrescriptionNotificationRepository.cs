using HealthInstitution.Core.PrescriptionNotifications.Model;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.PrescriptionNotifications.Repository;

public class PrescriptionNotificationRepository : IPrescriptionNotificationRepository
{
    private String _fileName = @"..\..\..\Data\recepieNotifications.json";
    public List<PrescriptionNotification> Notifications { get; set; }
    public Dictionary<int, PrescriptionNotification> NotificationsById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    public PrescriptionNotificationRepository()
    {
        this.Notifications = new List<PrescriptionNotification>();
        this.NotificationsById = new Dictionary<int, PrescriptionNotification>();
        this.LoadFromFile();
    }

    public void LoadFromFile()
    {
        var notifications = JsonSerializer.Deserialize<List<PrescriptionNotification>>(File.ReadAllText(@"..\..\..\Data\recepieNotifications.json"), _options);
        foreach (PrescriptionNotification notification in notifications)
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

    public PrescriptionNotification GetById(int id)
    {
        return this.NotificationsById[id];
    }

    public void Add(PrescriptionNotification recepieNotification)
    {
        this.Notifications.Add(recepieNotification);
        this.NotificationsById.Add(recepieNotification.Id, recepieNotification);
        Save();
    }

    public void Delete(int id)
    {
        PrescriptionNotification notification = NotificationsById[id];
        if (notification != null)
        {
            this.NotificationsById.Remove(notification.Id);
            this.Notifications.Remove(notification);
            Save();
        }
    }

    public List<PrescriptionNotification> GetPatientPresctiptionNotification(string username, int prescription)
    {
        List<PrescriptionNotification> ownNotifications = new List<PrescriptionNotification>();
        foreach (var notification in this.Notifications)
        {
            if (notification.Patient == username && notification.Prescription.Id == prescription) ownNotifications.Add(notification);
        }
        return ownNotifications;
    }

    public List<PrescriptionNotification> GetPatientActiveNotification(string username)
    {
        List<PrescriptionNotification> ownNotifications = new List<PrescriptionNotification>();
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

    public List<PrescriptionNotification> GetAll()
    {
        return Notifications;
    }

    public Dictionary<int, PrescriptionNotification> GetAllById()
    {
        return this.NotificationsById;
    }
}