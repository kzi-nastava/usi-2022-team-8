using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.PrescriptionNotifications.Model;

namespace HealthInstitution.Core.PrescriptionNotifications.Service;

public interface IPrescriptionNotificationService
{
    public void GenerateAllSkippedNotifications(string loggedPatient);

    public List<PrescriptionNotification> GetPatientActiveNotification(string username);

    public void GenerateCronJobs(List<DateTime> dateTimes, PrescriptionNotificationSettings setting, string loggedPatient);

    public List<DateTime> GenerateDateTimes(PrescriptionNotificationSettings setting)
    }