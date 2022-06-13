using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Prescriptions.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Prescriptions
{
    public class PrescriptionService : IPrescriptionService
    {
        IPrescriptionRepository _prescriptionRepository;
        public PrescriptionService(PrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }
        public List<Prescription> GetAll()
        {
            return _prescriptionRepository.GetAll();
        }

        public Prescription Add(PrescriptionDTO prescriptionDTO)
        {
            Prescription prescription = new Prescription(prescriptionDTO);
            return _prescriptionRepository.Add(prescription);
        }

        public void Update(int id, PrescriptionDTO prescriptionDTO)
        {
            Prescription prescription = new Prescription(prescriptionDTO);
            _prescriptionRepository.Update(id, prescription);
        }

        public void Delete(int id)
        {
            _prescriptionRepository.Delete(id);
        }

        public static bool IsPatientAlergic(MedicalRecord medicalRecord, List<Ingredient> ingredients)
        {
            return ingredients.Any(i => medicalRecord.Allergens.Contains(i.Name));
        }
    }
}