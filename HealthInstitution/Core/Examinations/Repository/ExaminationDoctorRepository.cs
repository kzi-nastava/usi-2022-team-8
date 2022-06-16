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
        private String _fileName = @"..\..\..\Data\examinationDoctor.json";
        private IDoctorRepository _doctorRepository;
        private IExaminationRepository _examinationRepository;

        public ExaminationDoctorRepository(IDoctorRepository doctorRepository, IExaminationRepository examinationRepository)
        {
            _doctorRepository = doctorRepository;
            _examinationRepository = examinationRepository;
            this.LoadFromFile();
        }

        public void LoadFromFile()
        {
            var doctorsByUsername = _doctorRepository.GetAllByUsername();
            var examinationsById = _examinationRepository.GetAllById();
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
            var examinations = _examinationRepository.GetAll();
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