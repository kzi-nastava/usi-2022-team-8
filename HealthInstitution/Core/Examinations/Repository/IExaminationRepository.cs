using HealthInstitution.Core.Examinations.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Examinations.Repository
{
    public interface IExaminationRepository : IRepository<Examination>
    {
        public Examination Parse(JToken? examination);
        public void LoadFromFile();
        public List<dynamic> PrepareForSerialization();
        public void Save();
        public List<Examination> GetAll();
        public Examination GetById(int id);
        public void AddToCollections(Examination examination);
        public void SaveAll();
        public void Add(Examination examination);
        public ExaminationDTO ParseExaminationToExaminationDTO(Examination examination);
        public void Update(int id, Examination byExamination);
        public void Delete(int id);
        public List<Examination> GetByPatient(string username);
        public List<Examination> GetCompletedByPatient(string patientUsername);
        public List<Examination> GetSeachAnamnesis(string keyword, string patientUsername);
        public void SwapExaminationValue(Examination examination);
        public Examination GenerateRequestExamination(Examination examination, string doctorUsername, DateTime dateTime);
        public void EditExamination(Examination examination, string doctorUsername, DateTime dateTime);
    }
}
