using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Prescriptions.Model;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.Prescriptions.Repository
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        public int maxId;
        private String _fileName;
        public List<Prescription> Prescriptions { get; set; }
        public Dictionary<int, Prescription> PrescriptionById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        private PrescriptionRepository(string fileName) //singleton
        {
            this.maxId = 0;
            this._fileName = fileName;
            this.Prescriptions = new List<Prescription>();
            this.PrescriptionById = new Dictionary<int, Prescription>();
            this.LoadFromFile();
        }

        private static PrescriptionRepository s_instance = null;

        public static PrescriptionRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new PrescriptionRepository(@"..\..\..\Data\JSON\prescriptions.json");
                }
                return s_instance;
            }
        }

        private Prescription Parse(JToken? prescription)
        {
            Dictionary<int, Drug> drugById = DrugRepository.GetInstance().DrugById;
            PrescriptionTime prescriptionTime;
            Enum.TryParse<PrescriptionTime>((string)prescription["timeOfUse"], out prescriptionTime);
            var dt = (string)prescription["hourlyRate"];
            string format = "MM/dd/yyyy HH:mm:ss";
            bool parse = DateTime.TryParseExact((string)prescription["hourlyRate"], format, null, DateTimeStyles.None, out var dateTime);
            return new Prescription((int)prescription["id"], (int)prescription["dailyDose"], prescriptionTime, drugById[(int)prescription["drug"]], dateTime);
        }

        public void LoadFromFile()
        {
            var prescriptions = JArray.Parse(File.ReadAllText(_fileName));
            //var prescriptions = JsonSerializer.Deserialize<List<Prescription>>(File.ReadAllText(@"..\..\..\Data\JSON\prescriptions.json"), _options);
            foreach (var prescription in prescriptions)
            {
                Prescription loadedPrescription = Parse(prescription);
                if (loadedPrescription.Id > maxId)
                {
                    maxId = loadedPrescription.Id;
                }
                this.Prescriptions.Add(loadedPrescription);
                this.PrescriptionById[loadedPrescription.Id] = loadedPrescription;
            }
        }

        private List<dynamic> PrepareForSerialization()
        {
            List<dynamic> reducedPrescriptions = new List<dynamic>();
            foreach (var prescription in this.Prescriptions)
            {
                reducedPrescriptions.Add(new
                {
                    id = prescription.Id,
                    dailyDose = prescription.DailyDose,
                    timeOfUse = prescription.TimeOfUse,
                    drug = prescription.Drug.Id
                });
            }
            return reducedPrescriptions;
        }

        public void Save()
        {
            var allPrescriptions = JsonSerializer.Serialize(PrepareForSerialization(), _options);
            File.WriteAllText(this._fileName, allPrescriptions);
        }

        public List<Prescription> GetAll()
        {
            return this.Prescriptions;
        }

        public Prescription GetById(int id)
        {
            if (PrescriptionById.ContainsKey(id))
                return PrescriptionById[id];
            return null;
        }

        public Prescription Add(Prescription prescription)
        {
            prescription.Id = ++maxId;
            this.Prescriptions.Add(prescription);
            this.PrescriptionById[prescription.Id] = prescription;
            Save();
            return prescription;
        }

        public void Update(int id, Prescription byPrescription)
        {
            Prescription prescription = GetById(id);
            prescription.DailyDose = byPrescription.DailyDose;
            prescription.TimeOfUse = byPrescription.TimeOfUse;
            prescription.Drug = byPrescription.Drug;
            prescription.HourlyRate = byPrescription.HourlyRate;
            PrescriptionById[id] = prescription;
            Save();
        }

        public void Delete(int id)
        {
            Prescription prescription = GetById(id);
            this.Prescriptions.Remove(prescription);
            this.PrescriptionById.Remove(id);
            Save();
        }
    }
}