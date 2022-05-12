using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.SystemUsers.Patients.Repository
{
    internal class PatientRepository
    {
        private String _fileName;
        public List<Patient> Patients { get; set; }
        public Dictionary<string, Patient> PatientByUsername { get; set; }

        UserRepository userRepository = UserRepository.GetInstance();

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
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
        public void LoadFromFile()
        {
            var patients = JsonSerializer.Deserialize<List<Patient>>(File.ReadAllText(@"..\..\..\Data\JSON\patients.json"), _options);
            foreach (Patient patient in patients)
            {
                this.Patients.Add(patient);
                this.PatientByUsername[patient.Username] = patient;
            }
        }

        public void Save()
        {
            var allPatients = JsonSerializer.Serialize(this.Patients, _options);
            File.WriteAllText(this._fileName, allPatients);
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

        public void Add(string username, string password, string name, string surname, double height, double weight, List<string> allergens, List<string> previousIlnesses)
        {
            Patient patient = new Patient(Users.Model.UserType.Patient, username, password, name, surname, Users.Model.BlockState.NotBlocked);
            
            MedicalRecordRepository medicalRecordRepository = MedicalRecordRepository.GetInstance();
            medicalRecordRepository.Add(height, weight, previousIlnesses, allergens, patient);
            this.Patients.Add(patient);
            this.PatientByUsername[username] = patient;
            Save();
        }

        public void Update(string username, string password, string name, string surname, Users.Model.BlockState blockState)
        {
            Patient patient = this.GetByUsername(username);
            patient.Password = password;
            patient.Name = name;
            patient.Surname = surname;
            patient.Blocked = blockState;
            this.PatientByUsername[username] = patient;
            Save();
            userRepository.Update(username, password, name, surname);
        }

        public void Delete(string username)
        {
            Patient patient = GetByUsername(username);
            this.Patients.Remove(patient);
            this.PatientByUsername.Remove(username);
            userRepository.Delete(username);
            Save();
        }
        public void ChangeBlockedStatus(string username)
        {
            Patient patient = this.GetByUsername(username);
            User user = userRepository.GetByUsername(username);
            if (patient.Blocked == Users.Model.BlockState.NotBlocked)
            {
                patient.Blocked = Users.Model.BlockState.BlockedBySecretary;
                user.Blocked = Users.Model.BlockState.BlockedBySecretary;
            } else
            {
                patient.Blocked = Users.Model.BlockState.NotBlocked;
                user.Blocked = Users.Model.BlockState.NotBlocked;
            }
            Save();
            userRepository.Save();
        }
        public void DeleteNotification(Patient patient, Notification notification)
        {
            patient.Notifications.Remove(notification);
            Save();
        }
    }
}
