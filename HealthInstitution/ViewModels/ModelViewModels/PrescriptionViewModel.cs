using HealthInstitution.Core;
using HealthInstitution.Core.Prescriptions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.ModelViewModels;

public class PrescriptionViewModel : ViewModelBase
{
    private Prescription _presritpion;

    public int Id => _presritpion.Id;
    public int DailyDose => _presritpion.DailyDose;
    public PrescriptionTime TimeOfUse => _presritpion.TimeOfUse;
    public string Drug => _presritpion.Drug.Name;
    public DateTime StartHours => _presritpion.HourlyRate;

    public PrescriptionViewModel(Prescription presritpion)
    {
        _presritpion = presritpion;
    }
}