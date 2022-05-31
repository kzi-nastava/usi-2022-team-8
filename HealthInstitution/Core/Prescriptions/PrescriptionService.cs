using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Prescriptions.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Prescriptions
{
    class PrescriptionService
    {
        private PrescriptionRepository _prescriptionRepository = PrescriptionRepository.GetInstance();
        public Prescription Add(PrescriptionDTO prescriptionDTO)
        {
            _prescriptionRepository.maxId++;
            int id = _prescriptionRepository.maxId;
            int dailyDose = prescriptionDTO.DailyDose;
            PrescriptionTime timeOfUse = prescriptionDTO.TimeOfUse;
            Drug drug = prescriptionDTO.Drug;

            Prescription prescription = new Prescription(id, dailyDose, timeOfUse, drug);
            _prescriptionRepository.Prescriptions.Add(prescription);
            _prescriptionRepository.PrescriptionById[id] = prescription;
            _prescriptionRepository.Save();
            return prescription;
        }

        public void Update(int id, PrescriptionDTO prescriptionDTO)
        {
            Prescription prescription = _prescriptionRepository.GetById(id);
            prescription.DailyDose = prescriptionDTO.DailyDose;
            prescription.TimeOfUse = prescriptionDTO.TimeOfUse;
            prescription.Drug = prescriptionDTO.Drug;
            _prescriptionRepository.PrescriptionById[id] = prescription;
            _prescriptionRepository.Save();
        }

        public void Delete(int id)
        {
            Prescription prescription = _prescriptionRepository.GetById(id);
            _prescriptionRepository.Prescriptions.Remove(prescription);
            _prescriptionRepository.PrescriptionById.Remove(id);
            _prescriptionRepository.Save();
        }
    }
}
