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
        private int maxId;
        public string fileName { get; set; }
        public List<Prescription> prescriptions { get; set; }
        public Dictionary<int, Prescription> prescriptionById { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private PrescriptionRepository(string fileName) //singleton
        {
            this.maxId = 0;
            this.fileName = fileName;
            this.prescriptions = new List<Prescription>();
            this.prescriptionById = new Dictionary<int, Prescription>();
            this.LoadPrescriptions();
        }
        private static PrescriptionRepository instance = null;
        public static PrescriptionRepository GetInstance()
        {
            {
                if (instance == null)
                {
                    instance = new PrescriptionRepository(@"..\..\..\Data\JSON\prescriptions.json");
                }
                return instance;
            }
        }
        public void LoadPrescriptions()
        {
            Dictionary<int, Drug> drugById = DrugRepository.GetInstance().drugById;
            var prescriptions = JArray.Parse(File.ReadAllText(fileName));
            //var prescriptions = JsonSerializer.Deserialize<List<Prescription>>(File.ReadAllText(@"..\..\..\Data\JSON\prescriptions.json"), options);
            foreach (var prescription in prescriptions)
            {
                PrescriptionTime prescriptionTime;
                Enum.TryParse<PrescriptionTime>((string)prescription["timeOfUse"], out prescriptionTime);

                Prescription prescriptionTemp = new Prescription((int)prescription["id"],
                                                                    (int)prescription["dailyDose"],
                                                                    prescriptionTime,
                                                                    drugById[(int)prescription["drug"]]
                                                                    );
                if (prescriptionTemp.id > maxId)
                {
                    maxId = prescriptionTemp.id;
                }
                this.prescriptions.Add(prescriptionTemp);
                this.prescriptionById[prescriptionTemp.id] = prescriptionTemp;
            }
        }
        public List<dynamic> ShortenPrescription()
        {
            List<dynamic> reducedPrescriptions = new List<dynamic>();
            foreach (var prescription in this.prescriptions)
            {
                reducedPrescriptions.Add(new
                {
                    id = prescription.id,
                    dailyDose = prescription.dailyDose,
                    timeOfUse = prescription.timeOfUse,
                    drug=prescription.drug.id
                }) ;
            }
            return reducedPrescriptions;
        }
        public void SavePrescriptions()
        {

            var allPrescriptions = JsonSerializer.Serialize(ShortenPrescription(), options);
            File.WriteAllText(this.fileName, allPrescriptions);
        }

        public List<Prescription> GetPrescriptions()
        {
            return this.prescriptions;
        }

        public Prescription GetPrescriptionById(int id)
        {
            if (prescriptionById.ContainsKey(id))
                return prescriptionById[id];
            return null;
        }

        public void AddPrescription(int dailyDose, PrescriptionTime timeOfUse, Drug drug)
        {
            this.maxId++;
            int id = this.maxId;
            Prescription prescription = new Prescription(id, dailyDose, timeOfUse, drug);
            this.prescriptions.Add(prescription);
            this.prescriptionById[id] = prescription;
            SavePrescriptions();
        }

        public void UpdatePrescription(int id, int dailyDose, PrescriptionTime timeOfUse, Drug drug)
        {
            Prescription prescription = GetPrescriptionById(id);
            prescription.dailyDose = dailyDose;
            prescription.timeOfUse = timeOfUse;
            prescription.drug = drug;
            prescriptionById[id] = prescription;
            SavePrescriptions();
        }

        public void DeletePrescription(int id)
        {
            Prescription prescription = GetPrescriptionById(id);
            this.prescriptions.Remove(prescription);
            this.prescriptionById.Remove(id);
            SavePrescriptions();
        }
    }
}
