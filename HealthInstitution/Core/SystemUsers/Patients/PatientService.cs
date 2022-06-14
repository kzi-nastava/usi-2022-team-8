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
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Operations;

namespace HealthInstitution.Core.SystemUsers.Patients
{
    public class PatientService : IPatientService
    {
        IPatientRepository _patientRepository;
        IUserService _userService;
        ITrollCounterService _trollCounterService;
        IMedicalRecordService _medicalRecordService;
        IExaminationService _examinationService;
        IOperationService _operationService;
        public PatientService(IPatientRepository patientRepository, IUserService userService, ITrollCounterService trollCounterService, IMedicalRecordService medicalRecordService)
        {
            _patientRepository = patientRepository;
            _medicalRecordService = medicalRecordService;
            _userService = userService;
            _trollCounterService = trollCounterService;
        }
        public void LoadNotifications()
        {
            AppointmentNotificationPatientRepository.GetInstance();
            AppointmentNotificationDoctorRepository.GetInstance();
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
            User user = _userService.GetByUsername(username);
            _patientRepository.ChangeBlockedStatus(patient);
            _userService.ChangeBlockedStatus(user);
        }
        public void Add(UserDTO userDTO, MedicalRecords.Model.MedicalRecordDTO medicalRecordDTO)
        {
            Patient patient = new Patient(userDTO);
            medicalRecordDTO.Patient = patient;
            _medicalRecordService.Add(medicalRecordDTO);
            _userService.Add(userDTO);
            _trollCounterService.Add(userDTO.Username);
            _patientRepository.Add(patient);
        }
        public void Update(UserDTO userDTO)
        {
            Patient patient = new Patient(userDTO);
            _patientRepository.Update(patient);
            _userService.Update(userDTO);
        }
        public void Delete(string username)
        {
            _patientRepository.Delete(username);
            _trollCounterService.Delete(username);
            _userService.Delete(username);
        }
        public void DeleteNotifications(Patient patient)
        {
            _patientRepository.DeleteNotifications(patient);
        }
        public bool IsAvailableForDeletion(Patient patient)
        {
            return _examinationService.GetByPatient(patient.Username).Count == 0 && _operationService.GetByPatient(patient.Username).Count() == 0;
        }
        public List<AppointmentNotification> GetActiveAppointmentNotification(Patient patient)
        {
            List<AppointmentNotification> appointmentNotifications=new List<AppointmentNotification>();
            foreach (AppointmentNotification appointmentNotification in patient.Notifications)
                if (appointmentNotification.ActiveForPatient)
                    appointmentNotifications.Add(appointmentNotification);
            return appointmentNotifications;
        }
    } 
}
