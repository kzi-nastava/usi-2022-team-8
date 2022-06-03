using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.RecepieNotifications.Repository;
using HealthInstitution.Core.Prescriptions.Repository;
using HealthInstitution.Core.RecepieNotifications.Model;

namespace HealthInstitution.Core.RecepieNotifications.Service;

public class PrescriptionNotificationService
{
    public static void GenerateAllSkippedNotifications(string loggedPatient)
    {
        foreach (var setting in PrescriptionNotificationSettingsRepository.GetInstance().Settings)
        {
            GenerateForOne(setting, loggedPatient);
        }
    }

    private static DateTime GetLastDateTime(PrescriptionNotificationSettings setting)
    {
        var createdNotifications = PrescriptionNotificationRepository.GetInstance().GetPatientPresctiptionNotification(setting.PatientUsername, setting.Prescription.Id);
        createdNotifications.OrderBy(o => o.TriggerDateTime).ToList();
        if (createdNotifications.Count == 0) return DateTime.Today.AddDays(-1);

        return createdNotifications.Last().TriggerDateTime;
    }

    private static double CalculateIncrement(PrescriptionNotificationSettings setting)
    {
        return 24 / setting.Prescription.DailyDose;
    }

    private static DateTime CalculateFirstDatetime(PrescriptionNotificationSettings setting)
    {
        DateTime lastDateTime = GetLastDateTime(setting);
        var firstDate = setting.Prescription.dateTime.AddHours(-setting.BeforeAmmount.Hour).AddMinutes(-setting.BeforeAmmount.Minute);
        lastDateTime = lastDateTime.AddMinutes(firstDate.Minute);
        lastDateTime = lastDateTime.AddHours(firstDate.Hour);
        return lastDateTime;
    }

    public static void GenerateCronJobs(List<DateTime> dateTimes, PrescriptionNotificationSettings setting, string loggedPatient)
    {
        foreach (DateTime dateTime in dateTimes)
            PrescriptionNotificationCronJobService.GenerateJob(loggedPatient, setting, dateTime);
    }

    private static void GenerateForOne(PrescriptionNotificationSettings setting, string loggedPatient)
    {
        List<DateTime> dateTimes = GenerateDateTimes(setting);
        GenerateCronJobs(dateTimes, setting, loggedPatient);

        while (true)
        {
            foreach (var dateTime in dateTimes)
            {
                if (dateTime > DateTime.Now) return;
                int id = PrescriptionNotificationRepository.GetInstance().Notifications.Count;
                PrescriptionNotification recepieNotification = new PrescriptionNotification(id, setting.PatientUsername, setting.Prescription, true);
                recepieNotification.TriggerDateTime = dateTime;
                PrescriptionNotificationRepository.GetInstance().Add(recepieNotification);
            }
            dateTimes = NextDay(dateTimes);
        }
    }

    private static List<DateTime> NextDay(List<DateTime> dateTimes)
    {
        for (int i = 0; i < dateTimes.Count; i++)
        {
            dateTimes[i] = dateTimes[i].AddDays(1);
        }
        return dateTimes;
    }

    public static List<DateTime> GenerateDateTimes(PrescriptionNotificationSettings setting)
    {
        double increment = CalculateIncrement(setting);
        List<DateTime> notificationTimes = new List<DateTime>();
        for (int i = 0; i < setting.Prescription.DailyDose; i++)
        {
            if (notificationTimes.Count == 0) notificationTimes.Add(CalculateFirstDatetime(setting));
            else notificationTimes.Add(notificationTimes.Last().AddHours(increment));
        }
        return notificationTimes;
    }

    public static List<PrescriptionNotification> GetPatientActiveNotification(string username)
    {
        return PrescriptionNotificationRepository.GetInstance().GetPatientActiveNotification(username);
    }
}