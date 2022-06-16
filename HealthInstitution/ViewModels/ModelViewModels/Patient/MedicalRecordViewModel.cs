using HealthInstitution.Core.MedicalRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.ModelViewModels.Patient
{
    public class MedicalRecordViewModel : ViewModelBase
    {
        private MedicalRecord _medicalRecord;
        public string Patient => _medicalRecord.Patient.Name + " " + _medicalRecord.Patient.Surname;
        public string Height => _medicalRecord.Height.ToString() + " cm";
        public string Weight => _medicalRecord.Weight.ToString() + " kg";
        public List<string> PreviousIllneses => _medicalRecord.PreviousIllnesses;
        public List<string> Allergens => _medicalRecord.Allergens;
        public MedicalRecordViewModel(MedicalRecord medicalRecord)
        {
            _medicalRecord = medicalRecord;
        }
    }
}