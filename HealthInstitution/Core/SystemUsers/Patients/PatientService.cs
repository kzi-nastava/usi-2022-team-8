using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users;
﻿using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;

namespace HealthInstitution.Core.SystemUsers.Patients
{
    internal static class PatientService
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
        public static void ChangeBlockedStatus(string username)
        {
            Patient patient = GetByUsername(username);
            User user = UserService.GetByUsername(username);
            s_patientRepository.ChangeBlockedStatus(patient);
            UserService.ChangeBlockedStatus(user);
            //dodati u usera
        }
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
        public static List<Operation> GetPatientOperations(Patient patient)
        {
            List<Operation> patientOperations = new List<Operation>();
            foreach (var operation in OperationRepository.GetInstance().GetAll())
                if (operation.MedicalRecord.Patient.Username == patient.Username)
                    patientOperations.Add(operation);
            return patientOperations;
        }
    } 
}
