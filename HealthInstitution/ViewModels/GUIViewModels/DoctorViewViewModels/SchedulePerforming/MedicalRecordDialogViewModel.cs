using HealthInstitution.Core;
using HealthInstitution.Core.MedicalRecords.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.SchedulePerforming
{
    public class MedicalRecordDialogViewModel : ViewModelBase
    {
        public string Patient { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public List<string> PreviousIllnesses { get; set; }
        public List<string> Allergens { get; set; }
        public MedicalRecordDialogViewModel(MedicalRecord medicalRecord)
        {
            Patient = medicalRecord.Patient.Name + " " + medicalRecord.Patient.Surname;
            Height = medicalRecord.Height.ToString() + " cm";
            Weight = medicalRecord.Weight.ToString() + " kg";
            PreviousIllnesses = medicalRecord.PreviousIllnesses;
            Allergens = medicalRecord.Allergens;
        }
    }
}
