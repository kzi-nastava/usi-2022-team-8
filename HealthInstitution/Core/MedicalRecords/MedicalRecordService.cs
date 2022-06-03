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
    static class MedicalRecordService
    {
        static MedicalRecordRepository s_medicalRecordRepository = MedicalRecordRepository.GetInstance();
        public static List<MedicalRecord> GetAll()
        {
            return s_medicalRecordRepository.GetAll();
        }

        public static MedicalRecord GetByPatientUsername(Patient patient)
        {
            return s_medicalRecordRepository.GetByPatientUsername(patient);
        }

        public static void Add(MedicalRecordDTO medicalRecordDTO)
        {
            MedicalRecord medicalRecord = new MedicalRecord(medicalRecordDTO);
            s_medicalRecordRepository.Add(medicalRecord);
        }

        public static void Update(MedicalRecordDTO medicalRecordDTO)
        {
            MedicalRecord medicalRecord = new MedicalRecord(medicalRecordDTO);
            s_medicalRecordRepository.Update(medicalRecord);
        }

        public static void AddReferral(Patient patient, Referral referral)
        {
            s_medicalRecordRepository.AddReferral(patient, referral);
        }

        public static void AddPrescription(Patient patient, Prescription prescription)
        {
            s_medicalRecordRepository.AddPrescription(patient, prescription);
        }
        public static void Delete(Patient patient)
        {
            s_medicalRecordRepository.Delete(patient);
        }

        public static void DeleteReferral(Patient patient, Referral referral)
        {
            s_medicalRecordRepository.DeleteReferral(patient, referral);    
        }

        public static void DeletePrescription(Patient patient, Prescription prescription)
        {
            s_medicalRecordRepository.DeletePrescription(patient, prescription);
        }
    }
}
