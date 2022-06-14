using HealthInstitution.Core.Examinations.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Examinations.Repository
{
    public interface IExaminationRepository
    {
        public void LoadFromFile();
        public void Save();
        public List<Examination> GetAll();
        public Dictionary<int, Examination> GetAllById();
        public Examination GetById(int id);
        public void Add(Examination examination);
        public void Update(int id, Examination byExamination);
        public void Delete(int id);
        public List<Examination> GetByPatient(string patientUsername);
        public List<Examination> GetByDoctor(string doctorUsername);
        public List<Examination> GetCompletedByPatient(string patientUsername);
        public List<Examination> GetSeachAnamnesis(string keyword, string patientUsername);
        public void SwapExaminationValue(Examination examination);
    }
}
