using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Referrals.Model;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        private MedicalRecordRepository(string fileName)
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
        public void LoadMedicalRecords()
        {
            var medicalRecords = JsonSerializer.Deserialize<List<MedicalRecord>>(File.ReadAllText(@"..\..\..\Data\JSON\medicalRecords.json"), options);
            foreach (MedicalRecord medicalRecord in medicalRecords)
            {
                this.medicalRecords.Add(medicalRecord);
                this.medicalRecordByUsername[medicalRecord.patientUsername] = medicalRecord;
            }
        }

        public void SaveMedicalRecords()
        {
            var allMedicalRecords = JsonSerializer.Serialize(this.medicalRecords, options);
            File.WriteAllText(this.fileName, allMedicalRecords);
        }
        
        public List<MedicalRecord> GetMedicalRecords()
        {
            return this.medicalRecords;
        }

        public MedicalRecord GetMedicalRecordByUsername(string username)
        {
            if (medicalRecordByUsername.ContainsKey(username))
                return medicalRecordByUsername[username];
            return null;
        }

        public void AddMedicalRecord(double height, double weight, List<string> previousIlnesses, List<string> allergens, string patientUsername)
        { 
            MedicalRecord medicalRecord = new MedicalRecord(height, weight, previousIlnesses, allergens, patientUsername);
            this.medicalRecords.Add(medicalRecord);
            this.medicalRecordByUsername[patientUsername]=medicalRecord;
            SaveMedicalRecords();
        }

        public void UpdateMedicalRecord(string username, double height, double weight, List<string> previousIlnesses, List<string> allergens, List<Prescription> prescriptions, List<Referral> referrals)
        {
            MedicalRecord medicalRecord = GetMedicalRecordByUsername(username);
            medicalRecord.height = height;
            medicalRecord.weight = weight;
            medicalRecord.prescriptions = prescriptions;
            medicalRecord.referrals = referrals;
            medicalRecord.previousIllnesses = previousIlnesses;
            medicalRecord.allergens = allergens;
            medicalRecordByUsername[username] = medicalRecord;
            SaveMedicalRecords();
        }

        public void DeleteMedicalRecord(string username)
        {
            MedicalRecord medicalRecord = GetMedicalRecordByUsername(username);
            this.medicalRecords.Remove(medicalRecord);
            this.medicalRecordByUsername.Remove(username);
            SaveMedicalRecords();
        }
    }
}
