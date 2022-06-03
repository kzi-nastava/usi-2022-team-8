using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;

namespace HealthInstitution.Core.SystemUsers.Doctors
{
    static class DoctorService
    {
        static DoctorRepository s_doctorRepository = DoctorRepository.GetInstance();
        public static List<Doctor> GetAll()
        {
            return s_doctorRepository.GetAll();
        }

        public static Doctor GetById(String username)
        {
            return s_doctorRepository.GetById(username);
        }

        public static void DeleteExamination(Examination examination)
        {
            s_doctorRepository.DeleteExamination(examination);
        }

        public static void DeleteOperation(Operation operation)
        {
            s_doctorRepository.DeleteOperation(operation);
        }
    }
}
