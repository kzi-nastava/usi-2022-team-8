using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Examinations.Repository
{
    internal class ExaminationDoctorRepository
    {
        public String fileName { get; set; }
        private ExaminationDoctorRepository(String fileName)
        {
            this.fileName = fileName;
            this.LoadFromFile();
        }

        private static ExaminationDoctorRepository instance = null;

        public static ExaminationDoctorRepository GetInstance()
        {
            {
                if (instance == null)
                {
                    instance = new ExaminationDoctorRepository(@"..\..\..\Data\JSON\examinationDoctor.json");
                }
                return instance;
            }
        }

        public void LoadFromFile()
        {
            var doctorsByUsername = DoctorRepository.GetInstance().doctorsByUsername;
            var examinationsById = ExaminationRepository.GetInstance().examinationsById;
            var examinationIdsDoctorUsernames = JArray.Parse(File.ReadAllText(this.fileName));
            foreach (var pair in examinationIdsDoctorUsernames)
            {
                int id = (int)pair["id"];
                String username = (String)pair["username"];
                Doctor doctor = doctorsByUsername[username];
                Examination examination = examinationsById[id];
                doctor.examinations.Add(examination);
                examination.doctor = doctor;
            }
        }

        public void SaveToFile()
        {
            List<dynamic> examinationIdsDoctorUsernames = new List<dynamic>();
            var examinations = ExaminationRepository.GetInstance().examinations;
            foreach (var examination in examinations)
            {
                Doctor doctor = examination.doctor;
                examinationIdsDoctorUsernames.Add(new { id = examination.id, username = doctor.username });
            }
            var allPairs = JsonSerializer.Serialize(examinationIdsDoctorUsernames);
            File.WriteAllText(this.fileName, allPairs);
        }
    }
}
