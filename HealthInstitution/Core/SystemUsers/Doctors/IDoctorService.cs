using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.SystemUsers.Doctors
{
    public interface IDoctorService
    {
        public List<Doctor> GetAll();
        public Doctor GetById(String username);
        public void DeleteExamination(Examination examination);
        public void DeleteOperation(Operation operation);
    }
}
