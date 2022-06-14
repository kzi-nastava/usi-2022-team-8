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
        public int GetMaxId();
        public List<Examination> GetAll();
        public Examination GetById(int id);
        public Examination Add(ExaminationDTO examinationDTO);
        public void Update(int id, ExaminationDTO examinationDTO);
        public void Delete(int id);
        public List<Examination> GetByPatient(string patientUsername);
        public List<Examination> GetByDoctor(string doctorUsername);
        public List<Examination> GetCompletedByPatient(string patientUsername);
        public List<Examination> GetSearchAnamnesis(string keyword, string patientUsername);
        public List<Examination> OrderByDoctorSpeciality(List<Examination> examinations);
        public List<Examination> OrderByDate(List<Examination> examinations);
        public List<Examination> OrderByDoctor(List<Examination> examinations);

        public bool IsReadyForPerforming(Examination examination);
        public ExaminationDTO ParseExaminationToExaminationDTO(Examination examination);
        public void Complete(Examination examination, string anamnesis);
    }
}
