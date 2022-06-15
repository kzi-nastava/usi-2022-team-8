using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
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
    public class MedicalRecordService : IMedicalRecordService
    {
        IMedicalRecordRepository _medicalRecordRepository;
        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
    }
        public List<MedicalRecord> GetAll()
        {
            return _medicalRecordRepository.GetAll();
        }

        public MedicalRecord GetByPatientUsername(Patient patient)
        {
            return _medicalRecordRepository.GetByPatientUsername(patient);
        }

        public void Add(MedicalRecordDTO medicalRecordDTO)
        {
            MedicalRecord medicalRecord = new MedicalRecord(medicalRecordDTO);
            _medicalRecordRepository.Add(medicalRecord);
        }

        public void Update(MedicalRecordDTO medicalRecordDTO)
        {
            MedicalRecord medicalRecord = new MedicalRecord(medicalRecordDTO);
            _medicalRecordRepository.Update(medicalRecord);
        }

        public void AddReferral(Patient patient, Referral referral)
        {
            _medicalRecordRepository.AddReferral(patient, referral);
        }

        public void AddPrescription(Patient patient, Prescription prescription)
        {
            _medicalRecordRepository.AddPrescription(patient, prescription);
        }
        public void Delete(Patient patient)
        {
            _medicalRecordRepository.Delete(patient);
        }

        public void DeleteReferral(Patient patient, Referral referral)
        {
            _medicalRecordRepository.DeleteReferral(patient, referral);    
        }

        public void DeletePrescription(Patient patient, Prescription prescription)
        {
            _medicalRecordRepository.DeletePrescription(patient, prescription);
        }

        public bool IsPatientAlergic(MedicalRecord medicalRecord, List<Ingredient> ingredients)
        {
            return ingredients.Any(i => medicalRecord.Allergens.Contains(i.Name));
        }
    }
}
