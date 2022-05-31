namespace HealthInstitution.Core.RecepieNotifications.Model;

public class RecepieNotificationSettings
{
    public RecepieNotificationSettings(DateTime beforeAmmount, string patientUsername, int prescriptionId, DateTime lastUpdated, string id)
    {
        BeforeAmmount = beforeAmmount;
        PatientUsername = patientUsername;
        PrescriptionId = prescriptionId;
        LastUpdated = lastUpdated;
        Id = id;
    }

    public DateTime BeforeAmmount { get; set; }
    public string PatientUsername { get; set; }
    public int PrescriptionId { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Id { get; set; }
}