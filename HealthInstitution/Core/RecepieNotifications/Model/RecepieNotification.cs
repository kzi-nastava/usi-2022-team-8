using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Prescriptions.Model;

namespace HealthInstitution.Core.RecepieNotifications.Model;

public class RecepieNotification
{
    public RecepieNotification(string id, Patient patient, Prescription prescription, bool activeForPatient)
    {
        Id = id;
        Patient = patient;
        Prescription = prescription;
        ActiveForPatient = activeForPatient;
    }

    public RecepieNotification()
    { }

    public string Id { get; set; }
    public string Patient { get; set; }
    public Prescription Prescription { get; set; }
    public bool ActiveForPatient { get; set; }
    public DateTime TriggerDateTime { get; set; }
}