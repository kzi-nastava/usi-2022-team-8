using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.RecepieNotifications.Repository;
using HealthInstitution.Core.Prescriptions.Repository;

namespace HealthInstitution.Core.RecepieNotifications.Model;

public class RecepieNotificationGenerator
{
    public void GenerateAllSkippedNotifications()
    {
        foreach (var setting in RecepieNotificationSettingsRepository.GetInstance().Settings)
        {
            GenerateForOne(setting);
        }
    }

    private DateTime GetLastDateTime(RecepieNotificationSettings setting)
    {
        var createdNotifications = RecepieNotificationRepository.GetInstance().GetPatientPresctiptionNotification(setting.PatientUsername, setting.PrescriptionId);
        createdNotifications.OrderBy(o => o.TriggerDateTime).ToList();
        if (createdNotifications.Count == 0) return DateTime.Today;

        return createdNotifications.Last().TriggerDateTime;
    }

    private double CalculateIncrement(RecepieNotificationSettings setting)
    {
        return 24 / PrescriptionRepository.GetInstance().GetById(setting.PrescriptionId).DailyDose;
    }

    private DateTime CalculateFirstDatetime(RecepieNotificationSettings setting)
    {
        DateTime lastDateTime = GetLastDateTime(setting);
        var firstDate = PrescriptionRepository.GetInstance().GetById(setting.PrescriptionId).dateTime.AddDays(-setting.BeforeAmmount.Hour).AddMinutes(-setting.BeforeAmmount.Minute);
        lastDateTime.AddMinutes(firstDate.Minute);
        lastDateTime.AddHours(firstDate.Hour);
        return lastDateTime;
    }

    private void GenerateForOne(RecepieNotificationSettings setting)
    {
        List<DateTime> dateTimes = GenerateDateTimes(setting);

        while (true)
        {
            foreach (var dateTime in dateTimes)
            {
                if (dateTime > DateTime.Now) return;
                int id = RecepieNotificationRepository.GetInstance().Notifications.Count;
                RecepieNotification recepieNotification = new RecepieNotification(id, setting.PatientUsername, PrescriptionRepository.GetInstance().GetById(setting.PrescriptionId), true);
                recepieNotification.TriggerDateTime = dateTime;
                RecepieNotificationRepository.GetInstance().Add(recepieNotification);
            }
            dateTimes = NextDay(dateTimes);
        }
    }

    private List<DateTime> NextDay(List<DateTime> dateTimes)
    {
        for (int i = 0; i < dateTimes.Count; i++)
        {
            dateTimes[i] = dateTimes[i].AddDays(1);
        }
        return dateTimes;
    }

    private List<DateTime> GenerateDateTimes(RecepieNotificationSettings setting)
    {
        double increment = CalculateIncrement(setting);
        List<DateTime> notificationTimes = new List<DateTime>();
        for (int i = 0; i < PrescriptionRepository.GetInstance().GetById(setting.PrescriptionId).DailyDose; i++)
        {
            if (notificationTimes.Count == 0) notificationTimes.Add(CalculateFirstDatetime(setting));
            else notificationTimes.Add(notificationTimes.Last().AddHours(increment));
        }
        return notificationTimes;
    }
}