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
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.Core.Notifications.Model;

namespace HealthInstitution.Core.SystemUsers.Patients
{
    public class PatientService : IPatientService
    {
        IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public List<Patient> GetAll()
        {
            return _patientRepository.GetAll();
        }
        public Patient GetByUsername(string username)
        {
            return _patientRepository.GetByUsername(username);
        }
        public void ChangeBlockedStatus(string username)
        {
            Patient patient = GetByUsername(username);
            User user = UserService.GetByUsername(username);
            _patientRepository.ChangeBlockedStatus(patient);
            UserService.ChangeBlockedStatus(user);
            //dodati u usera
        }
        public void Add(UserDTO userDTO, MedicalRecords.Model.MedicalRecordDTO medicalRecordDTO)
        {
            Patient patient = new Patient(userDTO);
            medicalRecordDTO.Patient = patient;
            MedicalRecordService.Add(medicalRecordDTO);
            UserService.Add(userDTO);
            TrollCounterService.Add(userDTO.Username);
            _patientRepository.Add(patient);
        }
        public void Update(UserDTO userDTO)
        {
            Patient patient = new Patient(userDTO);
            _patientRepository.Update(patient);
            UserService.Update(userDTO);
        }
        public void Delete(string username)
        {
            _patientRepository.Delete(username);
            TrollCounterService.Delete(username);
            UserService.Delete(username);
        }
        public void DeleteNotifications(Patient patient)
        {
            _patientRepository.DeleteNotifications(patient);
        }
    } 
}
