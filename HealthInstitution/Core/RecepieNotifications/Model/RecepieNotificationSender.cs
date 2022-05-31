using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.Prescriptions.Repository;
using HealthInstitution.Core.RecepieNotifications.Repository;

namespace HealthInstitution.Core.RecepieNotifications.Model;

public class RecepieNotificationSender : IJob
{
    public string _loggedUsername { get; set; }
    public RecepieNotificationSettings _settings { get; set; }

    public async Task Execute(IJobExecutionContext context)
    {
        _loggedUsername = (string)context.MergedJobDataMap["loggedUser"];
        _settings = (RecepieNotificationSettings)context.MergedJobDataMap["settings"];
        Int32 unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        RecepieNotification recepieNotification = new RecepieNotification(unixTimestamp, _settings.PatientUsername, _settings.Prescription, true);
        recepieNotification.TriggerDateTime = DateTime.Now;

        if (_loggedUsername == _settings.PatientUsername)
        {
            recepieNotification.ActiveForPatient = false;
            //TODO GenerateWindow
        }
        RecepieNotificationRepository.GetInstance().Add(recepieNotification);
    }
}