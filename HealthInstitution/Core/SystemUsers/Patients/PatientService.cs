using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Patients
{
    static class PatientService
    {
        static PatientRepository s_patientRepository = PatientRepository.GetInstance();
        public static List<Patient> GetAll()
        {
            return s_patientRepository.GetAll();
        }

        public static Patient GetByUsername(string username)
        {
            return s_patientRepository.GetByUsername(username);
        }

        //dodati u usera

        public static void Add(UserDTO userDTO, MedicalRecords.Model.MedicalRecordDTO medicalRecordDTO)
        {
            Patient patient = new Patient(userDTO);
            medicalRecordDTO.Patient = patient;
            MedicalRecordService.Add(medicalRecordDTO);
            s_patientRepository.Add(patient);
        }

        //updateovati u useru
        public static void Update(UserDTO userDTO)
        {
            Patient patient = new Patient(userDTO);
            s_patientRepository.Update(patient);
        }

        //ispravi delete

        public static void Delete(string username)
        {
            s_patientRepository.Delete(username);
            //TrollCounterFileRepository.GetInstance().Delete(username);
            //userRepository.Delete(username);
        }

        public static void ChangeBlockedStatus(string username)
        {
            s_patientRepository.ChangeBlockedStatus(username);
        }
    }
}
