using HealthInstitution.Core.Prescriptions.Model;

namespace HealthInstitution.Core.PrescriptionNotifications.Model;

public class PrescriptionNotification
{
    public PrescriptionNotification(int id, string patient, Prescription prescription, bool activeForPatient)
    {
        Id = id;
        Patient = patient;
        Prescription = prescription;
        ActiveForPatient = activeForPatient;
    }

    public PrescriptionNotification()
    { }

    public int Id { get; set; }
    public string Patient { get; set; }
    public Prescription Prescription { get; set; }
    public bool ActiveForPatient { get; set; }
    public DateTime TriggerDateTime { get; set; }
}