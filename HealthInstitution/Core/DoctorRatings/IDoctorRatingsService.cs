using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.DoctorRatings
{
    public interface IDoctorRatingsService
    {
        void Add(string username);
        void AssignScores();
        double GetAverageById(string id);
        List<Doctor> GetTopRated(int num);
        List<Doctor> GetWorstRated(int num);
        List<Doctor> LoadSortedDoctors();
        void UpdateScore(string DoctorUsername, int score);
    }
}