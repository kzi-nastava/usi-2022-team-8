using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.Prescriptions.Repository;
using HealthInstitution.Core.PrescriptionNotifications.Repository;

namespace HealthInstitution.Core.PrescriptionNotifications.Model;

public class PrescriptionNotificationSender : IJob
{
    public string _loggedUsername { get; set; }
    public PrescriptionNotificationSettings _settings { get; set; }

    public async Task Execute(IJobExecutionContext context)
    {
        _loggedUsername = (string)context.MergedJobDataMap["loggedUser"];
        _settings = (PrescriptionNotificationSettings)context.MergedJobDataMap["settings"];
        Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        PrescriptionNotification recepieNotification = new PrescriptionNotification(unixTimestamp, _settings.PatientUsername, _settings.Prescription, true);
        recepieNotification.TriggerDateTime = DateTime.Now;

        if (_loggedUsername == _settings.PatientUsername)
        {
            recepieNotification.ActiveForPatient = false;
            MessageBox.Show("Take " + recepieNotification.Prescription.Drug.Name + " at " + recepieNotification.TriggerDateTime + " " + recepieNotification.Prescription.TimeOfUse);
        }
        PrescriptionNotificationRepository.GetInstance().Add(recepieNotification);
    }
}