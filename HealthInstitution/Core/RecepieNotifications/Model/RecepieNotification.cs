using HealthInstitution.Core.Prescriptions.Model;

namespace HealthInstitution.Core.RecepieNotifications.Model;

public class RecepieNotification
{
    public RecepieNotification(int id, string patient, Prescription prescription, bool activeForPatient)
    {
        Id = id;
        Patient = patient;
        Prescription = prescription;
        ActiveForPatient = activeForPatient;
    }

    public RecepieNotification()
    { }

    public int Id { get; set; }
    public string Patient { get; set; }
    public Prescription Prescription { get; set; }
    public bool ActiveForPatient { get; set; }
    public DateTime TriggerDateTime { get; set; }
}