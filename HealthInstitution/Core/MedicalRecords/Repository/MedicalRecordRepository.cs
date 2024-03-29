﻿using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.Referrals.Repository;
using HealthInstitution.Core.Prescriptions.Repository;

namespace HealthInstitution.Core.MedicalRecords.Repository
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private String _fileName = @"..\..\..\Data\medicalRecords.json";

        private IPrescriptionRepository _prescriptionRepository;
        private IReferralRepository _referralRepository;
        private IPatientRepository _patientRepository;
        public List<MedicalRecord> MedicalRecords { get; set; }
        public Dictionary<string, MedicalRecord> MedicalRecordByUsername { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        public MedicalRecordRepository(IPrescriptionRepository prescriptionRepository, IReferralRepository referralRepository, IPatientRepository patientRepository)
        {
            _prescriptionRepository = prescriptionRepository;
            _referralRepository = referralRepository;
            _patientRepository = patientRepository;
            this.MedicalRecords = new List<MedicalRecord>();
            this.MedicalRecordByUsername = new Dictionary<string, MedicalRecord>();
            this.LoadFromFile();
        }

        private List<string> JToken2Strings(JToken tokens)
        {
            List<string> items = new List<string>();
            foreach (string token in tokens)
                items.Add(token);
            return items;
        }

        private List<Prescription> JToken2Prescriptions(JToken tokens)
        {
            Dictionary<int, Prescription> prescriptionById = _prescriptionRepository.GetAllById();
            List<Prescription> items = new List<Prescription>();
            foreach (int token in tokens)
                items.Add(prescriptionById[token]);
            return items;
        }

        private List<Referral> JToken2Referrals(JToken tokens)
        {
            Dictionary<int, Referral> referralById = _referralRepository.GetAllById();
            List<Referral> items = new List<Referral>();
            foreach (int token in tokens)
                items.Add(referralById[token]);
            return items;
        }

        private MedicalRecord Parse(JToken? medicalRecord)
        {
            Dictionary<string, Patient> patientByUsername = _patientRepository.GetAllByUsername();
            return new MedicalRecord((double)medicalRecord["height"],
                                                                    (double)medicalRecord["weight"],
                                                                    JToken2Strings(medicalRecord["previousIlnesses"]),
                                                                    JToken2Strings(medicalRecord["allergens"]),
                                                                    patientByUsername[(string)medicalRecord["patientUsername"]],
                                                                    JToken2Prescriptions(medicalRecord["prescriptionsId"]),
                                                                    JToken2Referrals(medicalRecord["referralsId"])
                                                                    );
        }

        public void LoadFromFile()
        {
            var medicalRecords = JArray.Parse(File.ReadAllText(_fileName));
            //var medicalRecords = JsonSerializer.Deserialize<List<MedicalRecord>>(File.ReadAllText(@"..\..\..\Data\medicalRecords.json"), _options);
            foreach (var medicalRecord in medicalRecords)
            {
                MedicalRecord loadedMedicalRecord = Parse(medicalRecord);
                this.MedicalRecords.Add(loadedMedicalRecord);
                this.MedicalRecordByUsername[loadedMedicalRecord.Patient.Username] = loadedMedicalRecord;
            }
        }

        private List<dynamic> PrepareForSerialization()
        {
            List<dynamic> reducedMedicalRecords = new List<dynamic>();
            foreach (var medicalRecord in this.MedicalRecords)
            {
                List<int> prescriptionsId = new List<int>();
                List<int> referralsId = new List<int>();
                foreach (var i in medicalRecord.Prescriptions)
                    prescriptionsId.Add(i.Id);
                foreach (var i in medicalRecord.Referrals)
                    referralsId.Add(i.Id);
                reducedMedicalRecords.Add(new
                {
                    height = medicalRecord.Height,
                    weight = medicalRecord.Weight,
                    patientUsername = medicalRecord.Patient.Username,
                    previousIlnesses = medicalRecord.PreviousIllnesses,
                    allergens = medicalRecord.Allergens,
                    prescriptionsId = prescriptionsId,
                    referralsId = referralsId
                });
            }
            return reducedMedicalRecords;
        }

        public void Save()
        {
            var allMedicalRecords = JsonSerializer.Serialize(PrepareForSerialization(), _options);
            File.WriteAllText(this._fileName, allMedicalRecords);
        }

        public List<MedicalRecord> GetAll()
        {
            return this.MedicalRecords;
        }

        public Dictionary<string, MedicalRecord> GetAllByUsername()
        {
            return this.MedicalRecordByUsername;
        }

        public MedicalRecord GetByPatientUsername(Patient patient)
        {
            if (MedicalRecordByUsername.ContainsKey(patient.Username))
                return MedicalRecordByUsername[patient.Username];
            return null;
        }

        public void Add(MedicalRecord medicalRecord)
        {
            this.MedicalRecords.Add(medicalRecord);
            this.MedicalRecordByUsername[medicalRecord.Patient.Username] = medicalRecord;
            Save();
        }

        public void Update(MedicalRecord byMedicalRecord)
        {
            MedicalRecord medicalRecord = GetByPatientUsername(byMedicalRecord.Patient);
            medicalRecord.Height = byMedicalRecord.Height;
            medicalRecord.Weight = byMedicalRecord.Weight;
            medicalRecord.Prescriptions = byMedicalRecord.Prescriptions;
            medicalRecord.Referrals = byMedicalRecord.Referrals;
            medicalRecord.PreviousIllnesses = byMedicalRecord.PreviousIllnesses;
            medicalRecord.Allergens = byMedicalRecord.Allergens;
            MedicalRecordByUsername[medicalRecord.Patient.Username] = medicalRecord;
            Save();
        }

        public void AddReferral(Patient patient, Referral referral)
        {
            MedicalRecord medicalRecord = GetByPatientUsername(patient);
            medicalRecord.Referrals.Add(referral);
            Save();
        }

        public void AddPrescription(Patient patient, Prescription prescription)
        {
            MedicalRecord medicalRecord = GetByPatientUsername(patient);
            medicalRecord.Prescriptions.Add(prescription);
            Save();
        }

        public void Delete(Patient patient)
        {
            MedicalRecord medicalRecord = GetByPatientUsername(patient);
            this.MedicalRecords.Remove(medicalRecord);
            this.MedicalRecordByUsername.Remove(patient.Username);
            Save();
        }

        public void DeleteReferral(Patient patient, Referral referral)
        {
            MedicalRecord medicalRecord = GetByPatientUsername(patient);
            medicalRecord.Referrals.Remove(referral);
            Save();
        }

        public void DeletePrescription(Patient patient, Prescription prescription)
        {
            MedicalRecord medicalRecord = GetByPatientUsername(patient);
            medicalRecord.Prescriptions.Remove(prescription);
            Save();
        }
    }
}