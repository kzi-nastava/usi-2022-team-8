using HealthInstitution.Core.Examinations.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Examinations
{
    public interface IExaminationService
    {
        public List<Examination> GetAll();
        public Examination GetById(int id);
        public void Add(ExaminationDTO examinationDTO);
        public void Update(int id, ExaminationDTO examinationDTO);
        public void Delete(int id);
        public List<Examination> GetByPatient(string username);
        public List<Examination> GetCompletedByPatient(string patientUsername);
        public List<Examination> GetSeachAnamnesis(string keyword, string patientUsername);
    }
}
