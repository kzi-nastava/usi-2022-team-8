using HealthInstitution.Core.MedicalRecords.Model;
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
    internal class MedicalRecordRepository
    {
        public string fileName { get; set; }
        public List<MedicalRecord> medicalRecords { get; set; }
        public Dictionary<string, MedicalRecord> medicalRecordByUsername { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private MedicalRecordRepository(string fileName) //singleton
        {
            this.fileName = fileName;
            this.medicalRecords = new List<MedicalRecord>();
            this.medicalRecordByUsername = new Dictionary<string, MedicalRecord>();
            this.LoadMedicalRecords();
        }
        private static MedicalRecordRepository instance = null;
        public static MedicalRecordRepository GetInstance()
        {
            {
                if (instance == null)
                {
                    instance = new MedicalRecordRepository(@"..\..\..\Data\JSON\medicalRecords.json");
                }
                return instance;
            }
        }
        public List<string> JToken2Strings(JToken tokens)
        {
            List<string> items = new List<string>();
            foreach(string token in tokens)
                items.Add(token);
            return items;
        }
        public List<Prescription> JToken2Prescriptions(JToken tokens)
        {
            Dictionary<int, Prescription> prescriptionById = PrescriptionRepository.GetInstance().prescriptionById;
            List<Prescription> items = new List<Prescription>();
            foreach (int token in tokens)
                items.Add(prescriptionById[token]);
            return items;
        }
        public List<Referral> JToken2Referrals(JToken tokens)
        {
            Dictionary<int, Referral> referralById = ReferralRepository.GetInstance().referralById;
            List<Referral> items = new List<Referral>();
            foreach (int token in tokens)
                items.Add(referralById[token]);
            return items;
        }
        public void LoadMedicalRecords()
        {
            Dictionary<string, Patient> patientByUsername = PatientRepository.GetInstance().patientByUsername;
            var medicalRecords = JArray.Parse(File.ReadAllText(fileName));
            //var medicalRecords = JsonSerializer.Deserialize<List<MedicalRecord>>(File.ReadAllText(@"..\..\..\Data\JSON\medicalRecords.json"), options);
            foreach (var medicalRecord in medicalRecords)
            {
                MedicalRecord medicalRecordTemp = new MedicalRecord((double)medicalRecord["height"],
                                                                    (double)medicalRecord["weight"],
                                                                    JToken2Strings(medicalRecord["previousIlnesses"]),
                                                                    JToken2Strings(medicalRecord["allergens"]),
                                                                    patientByUsername[(string)medicalRecord["patientUsername"]],
                                                                    JToken2Prescriptions(medicalRecord["prescriptionsId"]),
                                                                    JToken2Referrals(medicalRecord["referralsId"])
                                                                    );
                this.medicalRecords.Add(medicalRecordTemp);
                this.medicalRecordByUsername[medicalRecordTemp.patient.username] = medicalRecordTemp;
            }
        }
        public List<dynamic> ShortenMedicalRecord()
        {
            List <dynamic> reducedMedicalRecords = new List<dynamic>();
            foreach (var medicalRecord in this.medicalRecords)
            {
                List<int> prescriptionsId = new List<int>();
                List<int> referralsId = new List<int>();
                foreach (var i in medicalRecord.prescriptions)
                    prescriptionsId.Add(i.id);
                foreach (var i in medicalRecord.referrals)
                    referralsId.Add(i.id);
                reducedMedicalRecords.Add(new
                {
                    height = medicalRecord.height,
                    weight = medicalRecord.weight,
                    patientUsername = medicalRecord.patient.username,
                    previousIlnesses = medicalRecord.previousIllnesses,
                    allergens = medicalRecord.allergens,
                    prescriptionsId = prescriptionsId,
                    referralsId = referralsId
                });
            }
            return reducedMedicalRecords;
        }
        public void SaveMedicalRecords()
        {
            
            var allMedicalRecords = JsonSerializer.Serialize(ShortenMedicalRecord(), options);
            File.WriteAllText(this.fileName, allMedicalRecords);
        }
        
        public List<MedicalRecord> GetMedicalRecords()
        {
            return this.medicalRecords;
        }

        public MedicalRecord GetMedicalRecordByUsername(Patient patient)
        {
            if (medicalRecordByUsername.ContainsKey(patient.username))
                return medicalRecordByUsername[patient.username];
            return null;
        }

        public void AddMedicalRecord(double height, double weight, List<string> previousIlnesses, List<string> allergens, Patient patient)
        { 
            MedicalRecord medicalRecord = new MedicalRecord(height, weight, previousIlnesses, allergens, patient);
            this.medicalRecords.Add(medicalRecord);
            this.medicalRecordByUsername[patient.username]=medicalRecord;
            SaveMedicalRecords();
        }

        public void UpdateMedicalRecord(Patient patient, double height, double weight, List<string> previousIlnesses, List<string> allergens, List<Prescription> prescriptions, List<Referral> referrals)
        {
            MedicalRecord medicalRecord = GetMedicalRecordByUsername(patient);
            medicalRecord.height = height;
            medicalRecord.weight = weight;
            medicalRecord.prescriptions = prescriptions;
            medicalRecord.referrals = referrals;
            medicalRecord.previousIllnesses = previousIlnesses;
            medicalRecord.allergens = allergens;
            medicalRecordByUsername[patient.username] = medicalRecord;
            SaveMedicalRecords();
        }

        public void DeleteMedicalRecord(Patient patient)
        {
            MedicalRecord medicalRecord = GetMedicalRecordByUsername(patient);
            this.medicalRecords.Remove(medicalRecord);
            this.medicalRecordByUsername.Remove(patient.username);
            SaveMedicalRecords();
        }
    }
}
