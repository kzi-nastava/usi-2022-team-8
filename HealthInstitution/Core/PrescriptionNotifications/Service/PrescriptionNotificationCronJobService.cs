using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.PrescriptionNotifications.Model;
using HealthInstitution.Core.PrescriptionNotifications.Repository;
using Quartz;
using Quartz.Impl;

namespace HealthInstitution.Core.PrescriptionNotifications.Service;

public class PrescriptionNotificationCronJobService
{
    private static Dictionary<int, List<JobKey>> _jobKeysbyId = new();
    private static IScheduler _scheduler;

    public static void GenerateScheduler()
    {
        ISchedulerFactory schedFact = new StdSchedulerFactory();

        // get a scheduler
        IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        scheduler.Start();
    }

    public static void GenerateJob(string loggedUser, PrescriptionNotificationSettings settings, DateTime dateTime)
    {
        IJobDetail job = GenerateJob(settings, dateTime);
        job.JobDataMap.Put("loggedUser", loggedUser);
        job.JobDataMap.Put("settings", settings);

        ITrigger trigger = GenerateTrigger(settings, dateTime, job);
        _scheduler.ScheduleJob(job, trigger);

        AddToDictionary(settings.Id, job.Key);

        //"0 " + dateTime.Minute + " " + dateTime.Hour + " * * ?"
        //0 0/1 * * * ?
    }

    private static void AddToDictionary(int id, JobKey jobKey)
    {
        if (!_jobKeysbyId.ContainsKey(id))
            _jobKeysbyId[id] = new List<JobKey>();

        _jobKeysbyId[id].Add(jobKey);
    }

    private static ITrigger GenerateTrigger(PrescriptionNotificationSettings settings, DateTime dateTime, IJobDetail job)
    {
        return TriggerBuilder.Create()
      .WithIdentity("trigger" + settings.Id + dateTime, "group1")
      .WithCronSchedule("0 " + dateTime.Minute + " " + dateTime.Hour + " * * ?", x => x
          .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time")))
      .ForJob(job)
      .Build();
    }

    private static IJobDetail GenerateJob(PrescriptionNotificationSettings settings, DateTime dateTime)
    {
        return JobBuilder.Create<PrescriptionNotificationSender>()
       .WithIdentity("myJob" + settings.Id + dateTime, "group1") // name "myJob", group "group1"
       .Build();
    }
}