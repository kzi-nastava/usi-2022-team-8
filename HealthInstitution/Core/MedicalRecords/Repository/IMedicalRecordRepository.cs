using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.MedicalRecords.Repository
{
    public interface IMedicalRecordRepository
    {
        public void LoadFromFile();
        public void Save();
        public List<MedicalRecord> GetAll();
        public Dictionary<string, MedicalRecord> GetAllByUsername();
        public MedicalRecord GetByPatientUsername(Patient patient);
        public void Add(MedicalRecord medicalRecord);
        public void Update(MedicalRecord byMedicalRecord);
        public void AddReferral(Patient patient, Referral referral);
        public void AddPrescription(Patient patient, Prescription prescription);
        public void Delete(Patient patient);
        public void DeleteReferral(Patient patient, Referral referral);
        public void DeletePrescription(Patient patient, Prescription prescription);
    }
}
