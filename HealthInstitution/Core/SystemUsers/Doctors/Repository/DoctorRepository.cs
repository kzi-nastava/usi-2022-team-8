using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.SystemUsers.Doctors.Repository
{
    internal class DoctorRepository
    {
        private String _fileName;
        public List<Doctor> Doctors { get; set; }
        public Dictionary<String, Doctor> DoctorsByUsername { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        private DoctorRepository(String fileName)
        {
            this._fileName = fileName;
            this.Doctors = new List<Doctor>();
            this.DoctorsByUsername = new Dictionary<string, Doctor>();
            this.LoadFromFile();
        }

        private static DoctorRepository s_instance = null;
        public static DoctorRepository GetInstance()
        {
            if (s_instance == null)
            {
                s_instance = new DoctorRepository(@"..\..\..\Data\JSON\doctors.json");
            }
            return s_instance;
        }

        /*private List<Examination> convertJTokenToExamination(JToken tokens)
        {
            var examinationsById = ExaminationRepository.GetInstance().ExaminationsById;
            List<Examination> doctorExaminations = new List<Examination>();
            foreach (JToken token in tokens)
            {
                doctorExaminations.Add(examinationsById[(int)token]);
            }
            return doctorExaminations;
        }


        private List<Operation> convertJTokenToOperation(JToken tokens)
        {
            var operationsById = OperationRepository.GetInstance().OperationsById;
            List<Operation> doctorOperations = new List<Operation>();
            foreach (JToken token in tokens)
            {
                doctorOperations.Add(operationsById[(int)token]);
            }
            return doctorOperations;
        }*/

        public void LoadFromFile()
        {
            var allDoctors = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var doctor in allDoctors)
            {
                String username = (String)doctor["username"];
                String password = (String)doctor["password"];
                String name = (String)doctor["name"];
                String surname = (String)doctor["surname"];
                SpecialtyType specialtyType;
                Enum.TryParse(doctor["specialty"].ToString(), out specialtyType);
                Doctor loadedDoctor = new Doctor(username, password, name, surname, specialtyType);
                this.Doctors.Add(loadedDoctor);
                this.DoctorsByUsername.Add(username, loadedDoctor);
            }
        }

        /*public List<int> FormListOfExaminationIds(List<Examination> examinations)
        {
            List<int> ids = new List<int>();
            foreach (Examination examination in examinations)
                ids.Add(examination.Id);
            return ids;
        }

        public List<int> FormListOfOperationIds(List<Operation> operations)
        {
            List<int> ids = new List<int>();
            foreach (Operation operation in operations)
                ids.Add(operation.Id);
            return ids;
        }*/


        public void Save()
        {
            List<dynamic> reducedDoctors = new List<dynamic>();
            foreach (Doctor doctor in this.Doctors)
            {
                reducedDoctors.Add(new
                {
                    username = doctor.Username,
                    password = doctor.Password,
                    name = doctor.Name,
                    surname = doctor.Surname,
                    specialty = doctor.Specialty
                });
            }
            var allDoctors = JsonSerializer.Serialize(reducedDoctors, _options);
            File.WriteAllText(this._fileName, allDoctors);
        }

        public List<Doctor> GetAll()
        {
            return this.Doctors;
        }

        public Doctor GetById(String username)
        {
            if (this.DoctorsByUsername.ContainsKey(username))
                return this.DoctorsByUsername[username];
            return null;
        }

        public void DeleteExamination(Doctor doctor, Examination examination)
        {
            doctor.Examinations.Remove(examination);
            Save();
        }

        public void DeleteOperation(Doctor doctor, Operation operation)
        {
            doctor.Operations.Remove(operation);
            Save();
        }
    }
}
