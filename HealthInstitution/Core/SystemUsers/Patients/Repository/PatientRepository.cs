using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core.SystemUsers.Patients.Repository
{
    internal class PatientRepository
    {
        public string fileName { get; set; }
        public List<Patient> patients { get; set; }
        public Dictionary<string, Patient> patientByUsername { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private PatientRepository(string fileName)
        {
            this.fileName = fileName;
            this.patients = new List<Patient>();
            this.patientByUsername = new Dictionary<string, Patient>();
            this.LoadPatients();
        }
        private static PatientRepository instance = null;
        public static PatientRepository GetInstance()
        {
            {
                if (instance == null)
                {
                    instance = new PatientRepository(@"..\..\..\Data\JSON\patients.json");
                }
                return instance;
            }
        }
        public void LoadPatients()
        {
            var patients = JsonSerializer.Deserialize<List<Patient>>(File.ReadAllText(@"..\..\..\Data\JSON\patients.json"), options);
            foreach (Patient patient in patients)
            {
                this.patients.Add(patient);
                this.patientByUsername[patient.username] = patient;
            }
        }

        public void SavePatients()
        {
            var allPatients = JsonSerializer.Serialize(this.patients, options);
            File.WriteAllText(this.fileName, allPatients);
        }

        public List<Patient> GetPatients()
        {
            return this.patients;
        }

        public Patient GetPatientByUsername(string username)
        {
            if (patientByUsername.ContainsKey(username))
                return patientByUsername[username];
            return null;
        }

        public void AddPatient(string username, string password, string name, string surname, double height, double weight, List<string> allergens, List<string> previousIlnesses)
        {
            MedicalRecordRepository medicalRecordRepository = MedicalRecordRepository.GetInstance();
            Patient patient = new Patient(Users.Model.UserType.Patient, username, password, name, surname, Users.Model.BlockState.NotBlocked);
            medicalRecordRepository.AddMedicalRecord(height, weight,previousIlnesses, allergens,patient);
            this.patients.Add(patient);
            this.patientByUsername[username] = patient;
            SavePatients();
        }

        public void UpdatePatient(string username, string password, string name, string surname, Users.Model.BlockState blockState)
        {
            Patient patient = this.GetPatientByUsername(username);
            patient.password = password;
            patient.name = name;
            patient.surname = surname;
            patient.blocked = blockState;
            this.patientByUsername[username] = patient;
            SavePatients();
        }

        public void DeletePatient(string username)
        {
            Patient patient = GetPatientByUsername(username);
            this.patients.Remove(patient);
            this.patientByUsername.Remove(username);
            SavePatients();
        }
        public void ChangeBlockedStatus(string username)
        {
            Patient patient = this.GetPatientByUsername(username);
            if (patient.blocked == Users.Model.BlockState.NotBlocked)
                patient.blocked = Users.Model.BlockState.BlockedBySecretary;
            else
                patient.blocked = Users.Model.BlockState.NotBlocked;
        }
    }
}
