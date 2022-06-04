using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.PrescriptionNotifications.Model;

namespace HealthInstitution.Core.PrescriptionNotifications.Service;

public interface IPrescriptionNotificationCronJobService
{
    public void GenerateScheduler();

    public void GenerateJob(string loggedUser, PrescriptionNotificationSettings settings, DateTime dateTime);
}