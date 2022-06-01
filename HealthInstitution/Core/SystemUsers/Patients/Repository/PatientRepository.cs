﻿using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using Newtonsoft.Json.Linq;
using HealthInstitution.Core.TrollCounters.Repository;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using HealthInstitution.Core.TrollCounters;

namespace HealthInstitution.Core.SystemUsers.Patients.Repository
{
    internal class PatientRepository
    {
        private String _fileName;
        public List<Patient> Patients { get; set; }
        public Dictionary<string, Patient> PatientByUsername { get; set; }

        private UserRepository userRepository = UserRepository.GetInstance();

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        private PatientRepository(string fileName)
        {
            this._fileName = fileName;
            this.Patients = new List<Patient>();
            this.PatientByUsername = new Dictionary<string, Patient>();
            this.LoadFromFile();
        }

        private static PatientRepository s_instance = null;

        public static PatientRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new PatientRepository(@"..\..\..\Data\JSON\patients.json");
                }
                return s_instance;
            }
        }

        private Patient Parse(JToken? patient)
        {
            String username = (String)patient["username"];
            String password = (String)patient["password"];
            String name = (String)patient["name"];
            String surname = (String)patient["surname"];
            BlockState blockedState;
            Enum.TryParse(patient["blocked"].ToString(), out blockedState);
            Patient currentPatient = new Patient(UserType.Patient, username, password, name, surname);
            currentPatient.Blocked = blockedState;
            return currentPatient;
        }

        public void LoadFromFile()
        {
            var allPatients = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var patient in allPatients)
            {
                Patient loadedPatient = Parse(patient);
                this.Patients.Add(loadedPatient);
                this.PatientByUsername.Add(loadedPatient.Username, loadedPatient);
            }
        }

        private List<dynamic> PrepareForSerialization()
        {
            List<dynamic> reducedDoctors = new List<dynamic>();
            foreach (Patient patient in this.Patients)
            {
                reducedDoctors.Add(new
                {
                    type = patient.Type,
                    username = patient.Username,
                    password = patient.Password,
                    name = patient.Name,
                    surname = patient.Surname,
                    blocked = patient.Blocked
                });
            }
            return reducedDoctors;
        }

        public void Save()
        {
            var allDoctors = JsonSerializer.Serialize(PrepareForSerialization(), _options);
            File.WriteAllText(this._fileName, allDoctors);
        }

        public List<Patient> GetAll()
        {
            return this.Patients;
        }

        public Patient GetByUsername(string username)
        {
            if (PatientByUsername.ContainsKey(username))
                return PatientByUsername[username];
            return null;
        }

        public void Add(UserDTO userDTO, MedicalRecords.Model.MedicalRecordDTO medicalRecordDTO)
        {
            Patient patient = new Patient(userDTO.Type, userDTO.Username, userDTO.Password, userDTO.Name, userDTO.Surname);
            MedicalRecordRepository medicalRecordRepository = MedicalRecordRepository.GetInstance();

            medicalRecordDTO.Patient = patient;
            this.Patients.Add(patient);
            this.PatientByUsername[userDTO.Username] = patient;
            medicalRecordRepository.Add(medicalRecordDTO);
            Save();
        }

        public void Update(UserDTO userDTO)
        {
            Patient patient = this.GetByUsername(userDTO.Username);
            patient.Password = userDTO.Password;
            patient.Name = userDTO.Name;
            patient.Surname = userDTO.Surname;

            this.PatientByUsername[userDTO.Username] = patient;
            Save();
            userRepository.Update(userDTO);
        }

        public void Delete(string username)
        {
            Patient patient = GetByUsername(username);
            TrollCounterService.Delete(username);
            MedicalRecordRepository.GetInstance().Delete(patient);
            this.Patients.Remove(patient);
            this.PatientByUsername.Remove(username);
            userRepository.Delete(username);
            Save();
        }

        public void ChangeBlockedStatus(Patient patient)
        {
            if (patient.Blocked == BlockState.NotBlocked)
                patient.Blocked = BlockState.BlockedBySecretary;
            else
                patient.Blocked = BlockState.NotBlocked;
            Save();
        }

        public void DeleteNotification(Patient patient, AppointmentNotification notification)
        {
            patient.Notifications.Remove(notification);
            Save();
        }
    }
}