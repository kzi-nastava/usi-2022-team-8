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
    public class ExaminationDoctorRepository : IExaminationDoctorRepository
    {
        private String _fileName;

        private ExaminationDoctorRepository(String fileName)
        {
            this._fileName = fileName;
            this.LoadFromFile();
        }

        private static ExaminationDoctorRepository s_instance = null;

        public static ExaminationDoctorRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new ExaminationDoctorRepository(@"..\..\..\Data\JSON\examinationDoctor.json");
                }
                return s_instance;
            }
        }

        public void LoadFromFile()
        {
            var doctorsByUsername = DoctorRepository.GetInstance().DoctorsByUsername;
            var examinationsById = ExaminationRepository.GetInstance().ExaminationsById;
            var examinationIdsDoctorUsernames = JArray.Parse(File.ReadAllText(this._fileName));
            foreach (var pair in examinationIdsDoctorUsernames)
            {
                int id = (int)pair["id"];
                String username = (String)pair["username"];
                Doctor doctor = doctorsByUsername[username];
                Examination examination = examinationsById[id];
                doctor.Examinations.Add(examination);
                examination.Doctor = doctor;
            }
        }

        public void Save()
        {
            List<dynamic> examinationIdsDoctorUsernames = new List<dynamic>();
            var examinations = ExaminationRepository.GetInstance().Examinations;
            foreach (var examination in examinations)
            {
                Doctor doctor = examination.Doctor;
                examinationIdsDoctorUsernames.Add(new { id = examination.Id, username = doctor.Username });
            }
            var allPairs = JsonSerializer.Serialize(examinationIdsDoctorUsernames);
            File.WriteAllText(this._fileName, allPairs);
        }
    }
}