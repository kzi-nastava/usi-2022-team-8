using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Prescriptions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.ModelViewModels.Prescriptions
{
    public class PrescriptionViewModel : ViewModelBase
    {
        private Prescription _prescription;
        public int DailyDose => _prescription.DailyDose;
        public PrescriptionTime TimeOfUse => _prescription.TimeOfUse;
        public Drug Drug => _prescription.Drug;
        public DateTime HourlyRate => _prescription.HourlyRate;
        public PrescriptionViewModel(Prescription prescription)
        {
            _prescription = prescription;
        }
    }
}
