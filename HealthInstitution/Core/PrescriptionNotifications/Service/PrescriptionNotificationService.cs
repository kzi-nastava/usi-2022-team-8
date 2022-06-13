using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.PrescriptionNotifications.Repository;
using HealthInstitution.Core.Prescriptions.Repository;
using HealthInstitution.Core.PrescriptionNotifications.Model;

namespace HealthInstitution.Core.PrescriptionNotifications.Service;

public class PrescriptionNotificationService : IPrescriptionNotificationService
{

    IPrescriptionNotificationRepository _prescriptionNotificationRepository;
    IPrescriptionNotificationCronJobService _prescriptionNotificationCronJobService;
    IPrescriptionNotificationSettingsRepository _prescriptionNotificationSettingsRepository;
    public PrescriptionNotificationService(IPrescriptionNotificationRepository prescriptionNotificationRepository,
        IPrescriptionNotificationCronJobService prescriptionNotificationCronJobService,
        IPrescriptionNotificationSettingsRepository prescriptionNotificationSettingsRepository)
    {
        _prescriptionNotificationRepository = prescriptionNotificationRepository;
        _prescriptionNotificationCronJobService = prescriptionNotificationCronJobService;
        _prescriptionNotificationSettingsRepository = prescriptionNotificationSettingsRepository;
}
    public void GenerateAllSkippedNotifications(string loggedPatient)
    {
        _prescriptionNotificationCronJobService.GenerateScheduler();
        foreach (var setting in _prescriptionNotificationSettingsRepository.GetAll())
        {
            GenerateForOne(setting, loggedPatient);
        }
    }

    private DateTime GetLastDateTime(PrescriptionNotificationSettings setting)
    {
        var createdNotifications = _prescriptionNotificationRepository.GetPatientPresctiptionNotification(setting.PatientUsername, setting.Prescription.Id);
        createdNotifications.OrderBy(o => o.TriggerDateTime).ToList();
        if (createdNotifications.Count == 0) return DateTime.Today.AddDays(-1);

        return createdNotifications.Last().TriggerDateTime;
    }

    private double CalculateIncrement(PrescriptionNotificationSettings setting)
    {
        return 24 / setting.Prescription.DailyDose;
    }

    private DateTime CalculateFirstDatetime(PrescriptionNotificationSettings setting)
    {
        DateTime lastDateTime = GetLastDateTime(setting);
        var firstDate = setting.Prescription.HourlyRate.AddHours(-setting.BeforeAmmount.Hour).AddMinutes(-setting.BeforeAmmount.Minute);
        lastDateTime = lastDateTime.AddMinutes(firstDate.Minute);
        lastDateTime = lastDateTime.AddHours(firstDate.Hour);
        return lastDateTime;
    }

    public void GenerateCronJobs(List<DateTime> dateTimes, PrescriptionNotificationSettings setting, string loggedPatient)
    {
        foreach (DateTime dateTime in dateTimes)
            _prescriptionNotificationCronJobService.GenerateJob(loggedPatient, setting, dateTime);
    }

    private void GenerateForOne(PrescriptionNotificationSettings setting, string loggedPatient)
    {
        List<DateTime> dateTimes = GenerateDateTimes(setting);
        GenerateCronJobs(dateTimes, setting, loggedPatient);

        while (true)
        {
            foreach (var dateTime in dateTimes)
            {
                if (dateTime > DateTime.Now) return;
                int id = _prescriptionNotificationRepository.GetAll().Count;
                PrescriptionNotification recepieNotification = new PrescriptionNotification(id, setting.PatientUsername, setting.Prescription, true);
                recepieNotification.TriggerDateTime = dateTime;
                _prescriptionNotificationRepository.Add(recepieNotification);
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

    public List<DateTime> GenerateDateTimes(PrescriptionNotificationSettings setting)
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

    public List<PrescriptionNotification> GetPatientActiveNotification(string username)
    {
        return _prescriptionNotificationRepository.GetPatientActiveNotification(username);
    }

    public void UpdateSettings(int id, PrescriptionNotificationSettings settings)
    {
        _prescriptionNotificationSettingsRepository.Update(id, settings);
    }
}