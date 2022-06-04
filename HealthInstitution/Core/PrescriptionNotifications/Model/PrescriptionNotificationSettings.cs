using HealthInstitution.Core.Prescriptions.Model;

namespace HealthInstitution.Core.PrescriptionNotifications.Model;

public class PrescriptionNotificationSettings
{
    public PrescriptionNotificationSettings()
    { }

    public PrescriptionNotificationSettings(DateTime beforeAmmount, string patientUsername, Prescription prescription, DateTime lastUpdated, int id)
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
    public int Id { get; set; }
}