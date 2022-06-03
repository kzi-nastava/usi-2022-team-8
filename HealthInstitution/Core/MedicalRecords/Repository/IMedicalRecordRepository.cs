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
    public interface IMedicalRecordRepository : IRepository<MedicalRecord>
    {
        public List<string> JToken2Strings(JToken tokens);
        public List<Prescription> JToken2Prescriptions(JToken tokens);
        public List<Referral> JToken2Referrals(JToken tokens);
        public MedicalRecord Parse(JToken? medicalRecord);
        public void LoadFromFile();
        public List<dynamic> PrepareForSerialization();
        public void Save();
        public List<MedicalRecord> GetAll();
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
