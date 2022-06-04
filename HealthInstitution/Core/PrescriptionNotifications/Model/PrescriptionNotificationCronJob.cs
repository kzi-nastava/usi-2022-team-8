using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.PrescriptionNotifications.Model;

public class PrescriptionNotificationCronJob
{
    public void GenerateJob(string loggedUser, PrescriptionNotificationSettings settings, DateTime dateTime)
    {
        ISchedulerFactory schedFact = new StdSchedulerFactory();

        // get a scheduler
        IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        scheduler.Start();

        IJobDetail job = JobBuilder.Create<PrescriptionNotificationSender>()
        .WithIdentity("myJob" + settings.Id + dateTime, "group1") // name "myJob", group "group1"
        .Build();
        job.JobDataMap.Put("loggedUser", loggedUser);
        job.JobDataMap.Put("settings", settings);

        ITrigger trigger = TriggerBuilder.Create()
       .WithIdentity("trigger" + settings.Id + dateTime, "group1")
       .WithCronSchedule("0 0/1 * * * ?", x => x
           .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time")))
       .ForJob(job)
       .Build();
        scheduler.ScheduleJob(job, trigger);

        //"0 " + dateTime.Minute + " " + dateTime.Hour + " * * ?"
        //0 0/1 * * * ?
    }
}