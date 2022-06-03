using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Examinations
{
    internal static class ExaminationService
    {
        static ExaminationRepository s_examinationRepository = ExaminationRepository.GetInstance();
        public static List<Examination> GetAll()
        {
            return s_examinationRepository.GetAll();
        }
        public static Examination GetById(int id)
        {
            return s_examinationRepository.GetById(id);
        }

        public static Examination Add(ExaminationDTO examinationDTO)
        {
            Examination examination = new Examination(examinationDTO);
            s_examinationRepository.Add(examination);
            return examination;
        }

        public static void Update(int id, ExaminationDTO examinationDTO)
        {
            examinationDTO.Validate();
            Examination examination = new Examination(examinationDTO);
            DoctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
            PatientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
            s_examinationRepository.Update(id, examination);
        }

        public static void Delete(int id)
        {
            s_examinationRepository.Delete(id);
        }

        public static List<Examination> GetByPatient(string username)
        { 
           return s_examinationRepository.GetByPatient(username);
        }

        public static List<Examination> GetCompletedByPatient(string patientUsername)
        {
            return s_examinationRepository.GetCompletedByPatient(patientUsername);
        }

        public static List<Examination> GetSeachAnamnesis(string keyword, string patientUsername)
        {
            return s_examinationRepository.GetSeachAnamnesis(keyword, patientUsername);
        }

        
    }
}
