using HealthInstitution.Core.Prescriptions.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Prescriptions.Repository
{
    public interface IPrescriptionRepository : IRepository<Prescription>
    {
        public Prescription Parse(JToken? prescription);
        public void LoadFromFile();
        public List<dynamic> PrepareForSerialization();
        public void Save();
        public List<Prescription> GetAll();
        public Prescription GetById(int id);
        public Prescription Add(Prescription prescription);
        public void Update(int id, Prescription byPrescription);
        public void Delete(int id);
    }
}
