using HealthInstitution.Core;
using HealthInstitution.Core.PrescriptionNotifications.Model;
using HealthInstitution.Core.Prescriptions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.ModelViewModels;

public class PrescriptionNotificationViewModel : ViewModelBase
{
    private PrescriptionNotification _prescriptionNotification;

    public PrescriptionNotificationViewModel(PrescriptionNotification prescriptionNotification)
    {
        _prescriptionNotification = prescriptionNotification;
    }

    public string Id => _prescriptionNotification.Prescription.Drug.Name;
    public PrescriptionTime PrescriptionTime => _prescriptionNotification.Prescription.TimeOfUse;
    public int DailyDose => _prescriptionNotification.Prescription.DailyDose;
}