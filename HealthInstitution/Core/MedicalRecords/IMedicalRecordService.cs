using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.MedicalRecords
{
    public interface IMedicalRecordService
    {
        public List<MedicalRecord> GetAll();
        public MedicalRecord GetByPatientUsername(Patient patient);
        public void Add(MedicalRecordDTO medicalRecordDTO);
        public void Update(MedicalRecordDTO medicalRecordDTO);
        public void AddReferral(Patient patient, Referral referral);
        public void AddPrescription(Patient patient, Prescription prescription);
        public void Delete(Patient patient);
        public void DeleteReferral(Patient patient, Referral referral);
        public void DeletePrescription(Patient patient, Prescription prescription);
        public bool IsPatientAlergic(MedicalRecord medicalRecord, List<Ingredient> ingredients);
    }
}
