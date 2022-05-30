using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.MedicalRecords.Model;

public class RecepieNotificationSender : IJob
{
    Task IJob.Execute(IJobExecutionContext context)
    {
        throw new NotImplementedException();
    }
}