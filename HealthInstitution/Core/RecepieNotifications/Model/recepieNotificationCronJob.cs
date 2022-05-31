using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RecepieNotifications.Model;

public class recepieNotificationCronJob
{
    public void GenerateJob(string loggedUser, RecepieNotificationSettings settings, DateTime dateTime)
    {
        StdSchedulerFactory factory = new StdSchedulerFactory();
        IScheduler scheduler = (IScheduler)factory.GetScheduler();
        scheduler.Start();

        IJobDetail job = JobBuilder.Create<RecepieNotificationSender>()
        .WithIdentity("myJob" + settings.Id + dateTime, "group1") // name "myJob", group "group1"
        .Build();
        job.JobDataMap.Put("loggedUser", loggedUser);
        job.JobDataMap.Put("settings", settings);

        ITrigger trigger = TriggerBuilder.Create()
       .WithIdentity("trigger3", "group1")
       .WithCronSchedule("0 " + dateTime.Minute + " " + dateTime.Hour + " * * *", x => x
           .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time")))
       .ForJob(job)
       .Build();

        scheduler.ScheduleJob(job, trigger);
    }
}