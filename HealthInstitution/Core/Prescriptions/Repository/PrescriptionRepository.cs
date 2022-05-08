using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Prescriptions.Model;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.Prescriptions.Repository
{
    internal class PrescriptionRepository
    {
        private int _maxId;
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
            this._maxId = 0;
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
        public void LoadFromFile()
        {
            Dictionary<int, Drug> drugById = DrugRepository.GetInstance().DrugById;
            var prescriptions = JArray.Parse(File.ReadAllText(_fileName));
            //var prescriptions = JsonSerializer.Deserialize<List<Prescription>>(File.ReadAllText(@"..\..\..\Data\JSON\prescriptions.json"), _options);
            foreach (var prescription in prescriptions)
            {
                PrescriptionTime prescriptionTime;
                Enum.TryParse<PrescriptionTime>((string)prescription["timeOfUse"], out prescriptionTime);

                Prescription prescriptionTemp = new Prescription((int)prescription["id"],
                                                                    (int)prescription["dailyDose"],
                                                                    prescriptionTime,
                                                                    drugById[(int)prescription["drug"]]
                                                                    );
                if (prescriptionTemp.Id > _maxId)
                {
                    _maxId = prescriptionTemp.Id;
                }
                this.Prescriptions.Add(prescriptionTemp);
                this.PrescriptionById[prescriptionTemp.Id] = prescriptionTemp;
            }
        }
        private List<dynamic> ShortenPrescription()
        {
            List<dynamic> reducedPrescriptions = new List<dynamic>();
            foreach (var prescription in this.Prescriptions)
            {
                reducedPrescriptions.Add(new
                {
                    id = prescription.Id,
                    dailyDose = prescription.DailyDose,
                    timeOfUse = prescription.TimeOfUse,
                    drug=prescription.Drug.Id
                }) ;
            }
            return reducedPrescriptions;
        }
        public void Save()
        {

            var allPrescriptions = JsonSerializer.Serialize(ShortenPrescription(), _options);
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

        public Prescription Add(int dailyDose, PrescriptionTime timeOfUse, Drug drug)
        {
            this._maxId++;
            int id = this._maxId;
            Prescription prescription = new Prescription(id, dailyDose, timeOfUse, drug);
            this.Prescriptions.Add(prescription);
            this.PrescriptionById[id] = prescription;
            Save();
            return prescription;
        }

        public void Update(int id, int dailyDose, PrescriptionTime timeOfUse, Drug drug)
        {
            Prescription prescription = GetById(id);
            prescription.DailyDose = dailyDose;
            prescription.TimeOfUse = timeOfUse;
            prescription.Drug = drug;
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
