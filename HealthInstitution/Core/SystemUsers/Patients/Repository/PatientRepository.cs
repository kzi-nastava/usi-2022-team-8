using HealthInstitution.Core.MedicalRecords.Repository;
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
using HealthInstitution.Core.MedicalRecords;

namespace HealthInstitution.Core.SystemUsers.Patients.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private String _fileName = @"..\..\..\Data\patients.json";
        public List<Patient> Patients { get; set; }
        public Dictionary<string, Patient> PatientByUsername { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        public PatientRepository()
        {
            this.Patients = new List<Patient>();
            this.PatientByUsername = new Dictionary<string, Patient>();
            this.LoadFromFile();
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

        public Dictionary<string, Patient> GetAllByUsername()
        {
            return PatientByUsername;
        }

        public Patient GetByUsername(string username)
        {
            if (PatientByUsername.ContainsKey(username))
                return PatientByUsername[username];
            return null;
        }

        public void Add(Patient patient)
        {
            this.Patients.Add(patient);
            this.PatientByUsername[patient.Username] = patient;
            Save();
        }

        public void Update(Patient byPatient)
        {
            Patient patient = GetByUsername(byPatient.Username);
            patient.Password = byPatient.Password;
            patient.Name = byPatient.Name;
            patient.Surname = byPatient.Surname;
            this.PatientByUsername[patient.Username] = patient;
            Save();
        }

        public void Delete(string username)
        {
            Patient patient = GetByUsername(username);
            this.Patients.Remove(patient);
            this.PatientByUsername.Remove(username);
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

        public void DeleteNotifications(Patient patient)
        {
            patient.Notifications.Clear();
            Save();
        }
    }
}