using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Prescriptions.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Prescriptions
{
    public static class PrescriptionService
    {
        static PrescriptionRepository s_prescriptionRepository = PrescriptionRepository.GetInstance();
        public static List<Prescription> GetAll()
        {
            return s_prescriptionRepository.GetAll();
        }

        public static Prescription Add(PrescriptionDTO prescriptionDTO)
        {
            Prescription prescription = new Prescription(prescriptionDTO);
            return s_prescriptionRepository.Add(prescription);
        }

        public static void Update(int id, PrescriptionDTO prescriptionDTO)
        {
            Prescription prescription = new Prescription(prescriptionDTO);
            s_prescriptionRepository.Update(id, prescription);
        }

        public static void Delete(int id)
        {
            s_prescriptionRepository.Delete(id);
        }
    }
}
