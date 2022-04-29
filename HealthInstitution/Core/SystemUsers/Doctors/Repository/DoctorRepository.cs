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
        public String fileName { get; set; }
        public List<Doctor> doctors { get; set; }
        public Dictionary<String, Doctor> doctorsByUsername { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        private DoctorRepository(String fileName)
        {
            this.fileName = fileName;
            this.doctors = new List<Doctor>();
            this.doctorsByUsername = new Dictionary<string, Doctor>();
            this.LoadDoctors();
        }

        private static DoctorRepository instance = null;
        public static DoctorRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new DoctorRepository(@"..\..\..\Data\JSON\doctors.json");
            }
            return instance;
        }

        public List<Examination> ConvertJTokenToExamination(JToken tokens)
        {
            var examinationsById = ExaminationRepository.GetInstance().examinationsById;
            List<Examination> doctorExaminations = new List<Examination>();
            foreach (JToken token in tokens)
            {
                doctorExaminations.Add(examinationsById[(int)token]);
            }
            return doctorExaminations;
        }


        public List<Operation> ConvertJTokenToOperation(JToken tokens)
        {
            var operationsById = OperationRepository.GetInstance().operationsById;
            List<Operation> doctorOperations = new List<Operation>();
            foreach (JToken token in tokens)
            {
                doctorOperations.Add(operationsById[(int)token]);
            }
            return doctorOperations;
        }

        public void LoadDoctors()
        {
            var doctors = JArray.Parse(File.ReadAllText(this.fileName));
            foreach (var doctor in doctors)
            {
                String username = (String)doctor["username"];
                String password = (String)doctor["password"];
                String name = (String)doctor["name"];
                String surname = (String)doctor["surname"];
                SpecialtyType specialtyType;
                Enum.TryParse(doctor["specialty"].ToString(), out specialtyType);
                //List<Examination> doctorExaminations = ConvertJTokenToExamination(doctor["examination"]);
                //List<Operation> doctorOperations = ConvertJTokenToOperation(doctor["operations"]);
                Doctor loadedDoctor = new Doctor(username, password, name, surname, specialtyType);
                this.doctors.Add(loadedDoctor);
                this.doctorsByUsername.Add(username, loadedDoctor);
            }
        }

        public List<int> FormListOfExaminationIds(List<Examination> examinations)
        {
            List<int> ids = new List<int>();
            foreach (Examination examination in examinations)
                ids.Add(examination.id);
            return ids;
        }

        public List<int> FormListOfOperationIds(List<Operation> operations)
        {
            List<int> ids = new List<int>();
            foreach (Operation operation in operations)
                ids.Add(operation.id);
            return ids;
        }

        public List<dynamic> ShortenDoctor()
        {
            List<dynamic> reducedDoctors = new List<dynamic>();
            foreach (Doctor doctor in this.doctors)
            {
                reducedDoctors.Add(new
                {
                    username = doctor.username,
                    password = doctor.password,
                    name = doctor.name,
                    surname = doctor.surname,
                    specialty = doctor.specialty,
                    examinations = FormListOfExaminationIds(doctor.examinations),
                    operations = FormListOfOperationIds(doctor.operations)
                });
            }
            return reducedDoctors;
        }

        public void SaveDoctors()
        {
            var allDoctors = JsonSerializer.Serialize(ShortenDoctor(), options);
            File.WriteAllText(this.fileName, allDoctors);
        }

        public List<Doctor> GetDoctors()
        {
            return this.doctors;
        }

        public Doctor GetDoctorByUsername(String username)
        {
            if (this.doctorsByUsername.ContainsKey(username))
                return this.doctorsByUsername[username];
            return null;
        }

        public void DeleteDoctorExamination(Doctor doctor, Examination examination)
        {
            doctor.examinations.Remove(examination);
            SaveDoctors();
        }

        public void DeleteDoctorOperation(Doctor doctor, Operation operation)
        {
            doctor.operations.Remove(operation);
            SaveDoctors();
        }
    }
}
