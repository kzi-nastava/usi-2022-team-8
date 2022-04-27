using HealthInstitution.Core.Prescriptions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Prescriptions.Repository
{
    internal class PrescriptionRepository
    {
        public string fileName { get; set; }
        public List<Prescription> prescriptions { get; set; }
        public Dictionary<int, Prescription> prescriptionById { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private PrescriptionRepository(string fileName) //singleton
        {
            this.fileName = fileName;
            this.prescriptions = new List<Prescription>();
            this.prescriptionById = new Dictionary<int, Prescription>();
            this.LoadPrescription();
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

    }
}
