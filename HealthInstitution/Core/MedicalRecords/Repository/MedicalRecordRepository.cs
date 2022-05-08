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
        private String _fileName;
        public List<MedicalRecord> MedicalRecords { get; set; }
        public Dictionary<string, MedicalRecord> MedicalRecordByUsername { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
        private MedicalRecordRepository(string fileName) //singleton
        {
            this._fileName = fileName;
            this.MedicalRecords = new List<MedicalRecord>();
            this.MedicalRecordByUsername = new Dictionary<string, MedicalRecord>();
            this.LoadFromFile();
        }
        private static MedicalRecordRepository s_instance = null;
        public static MedicalRecordRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new MedicalRecordRepository(@"..\..\..\Data\JSON\medicalRecords.json");
                }
                return s_instance;
            }
        }
        private List<string> JToken2Strings(JToken tokens)
        {
            List<string> items = new List<string>();
            foreach(string token in tokens)
                items.Add(token);
            return items;
        }
        private List<Prescription> JToken2Prescriptions(JToken tokens)
        {
            Dictionary<int, Prescription> prescriptionById = PrescriptionRepository.GetInstance().PrescriptionById;
            List<Prescription> items = new List<Prescription>();
            foreach (int token in tokens)
                items.Add(prescriptionById[token]);
            return items;
        }
        private List<Referral> JToken2Referrals(JToken tokens)
        {
            Dictionary<int, Referral> referralById = ReferralRepository.GetInstance().ReferralById;
            List<Referral> items = new List<Referral>();
            foreach (int token in tokens)
                items.Add(referralById[token]);
            return items;
        }
        public void LoadFromFile()
        {
            Dictionary<string, Patient> patientByUsername = PatientRepository.GetInstance().PatientByUsername;
            var medicalRecords = JArray.Parse(File.ReadAllText(_fileName));
            //var medicalRecords = JsonSerializer.Deserialize<List<MedicalRecord>>(File.ReadAllText(@"..\..\..\Data\JSON\medicalRecords.json"), _options);
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
                this.MedicalRecords.Add(medicalRecordTemp);
                this.MedicalRecordByUsername[medicalRecordTemp.Patient.Username] = medicalRecordTemp;
            }
        }
        private List<dynamic> ShortenMedicalRecord()
        {
            List <dynamic> reducedMedicalRecords = new List<dynamic>();
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
            
            var allMedicalRecords = JsonSerializer.Serialize(ShortenMedicalRecord(), _options);
            File.WriteAllText(this._fileName, allMedicalRecords);
        }
        
        public List<MedicalRecord> GetAll()
        {
            return this.MedicalRecords;
        }

        public MedicalRecord GetByPatientUsername(Patient patient)
        {
            if (MedicalRecordByUsername.ContainsKey(patient.Username))
                return MedicalRecordByUsername[patient.Username];
            return null;
        }

        public void Add(double height, double weight, List<string> previousIlnesses, List<string> allergens, Patient patient)
        { 
            MedicalRecord medicalRecord = new MedicalRecord(height, weight, previousIlnesses, allergens, patient);
            this.MedicalRecords.Add(medicalRecord);
            this.MedicalRecordByUsername[patient.Username]=medicalRecord;
            Save();
        }

        public void Update(Patient patient, double height, double weight, List<string> previousIlnesses, List<string> allergens, List<Prescription> prescriptions, List<Referral> referrals)
        {
            MedicalRecord medicalRecord = GetByPatientUsername(patient);
            medicalRecord.Height = height;
            medicalRecord.Weight = weight;
            medicalRecord.Prescriptions = prescriptions;
            medicalRecord.Referrals = referrals;
            medicalRecord.PreviousIllnesses = previousIlnesses;
            medicalRecord.Allergens = allergens;
            MedicalRecordByUsername[patient.Username] = medicalRecord;
            Save();
        }

        public void Delete(Patient patient)
        {
            MedicalRecord medicalRecord = GetByPatientUsername(patient);
            this.MedicalRecords.Remove(medicalRecord);
            this.MedicalRecordByUsername.Remove(patient.Username);
            Save();
        }
    }
}
