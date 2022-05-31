using HealthInstitution.Core.Prescriptions.Model;

namespace HealthInstitution.Core.RecepieNotifications.Model;

public class RecepieNotificationSettings
{
    public RecepieNotificationSettings(DateTime beforeAmmount, string patientUsername, Prescription prescription, DateTime lastUpdated, string id)
    {
        BeforeAmmount = beforeAmmount;
        PatientUsername = patientUsername;
        Prescription = prescription;
        LastUpdated = lastUpdated;
        Id = id;
    }

    public DateTime BeforeAmmount { get; set; }
    public string PatientUsername { get; set; }
    public Prescription Prescription { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Id { get; set; }
}