using HealthInstitution.Core.Prescriptions.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Prescriptions.Repository
{
    public interface IPrescriptionRepository
    {
        public void LoadFromFile();
        public void Save();
        public List<Prescription> GetAll();
        public Prescription GetById(int id);
        public Prescription Add(Prescription prescription);
        public void Update(int id, Prescription byPrescription);
        public void Delete(int id);
    }
}
