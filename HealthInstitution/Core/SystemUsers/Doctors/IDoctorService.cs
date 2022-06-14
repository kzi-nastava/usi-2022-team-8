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
        public void AssignScorebyId(string username, double avgRating);
        public List<Doctor> OrderByDoctorRating(List<Doctor> examinations);
        public List<Doctor> OrderByDoctorSpeciality(List<Doctor> examinations);
        public List<Doctor> OrderByDoctorName(List<Doctor> examinations);
        public List<Doctor> OrderByDoctorSurname(List<Doctor> examinations);
        public List<Doctor> SearchBySpeciality(string keyword);
        public List<Doctor> SearchByName(string keyword);
        public List<Doctor> SearchBySurname(string keyword);
        public void LoadAppointments();
        public void LoadNotifications();
        public void DeleteNotifications(Doctor doctor);

    }
}
