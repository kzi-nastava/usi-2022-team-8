using HealthInstitution.Core.Prescriptions.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Prescriptions
{
    public interface IPrescriptionService
    {
        public List<Prescription> GetAll();
        public Prescription Add(PrescriptionDTO prescriptionDTO);
        public void Update(int id, PrescriptionDTO prescriptionDTO);
        public void Delete(int id);
    }
}
