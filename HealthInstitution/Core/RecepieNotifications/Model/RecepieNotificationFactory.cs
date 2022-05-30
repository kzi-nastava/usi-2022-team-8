using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Prescriptions.Model;

namespace HealthInstitution.Core.RecepieNotifications.Model;

public class RecepieNotificationFactory
{
    private DateTime BeforeAmmount { get; set; }
    public string PatientUsername { get; set; }
    public int PrescriptionId { get; set; }
}