using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Polls.Repository
{
    public interface IPollQuestionRepository
    {
        void Add(PollQuestion pollQuestion);
        void Delete(int id);
        List<PollQuestion> GetAll();
        Dictionary<int, PollQuestion> GetAllById();
        PollQuestion GetById(int id);
        List<PollQuestion> GetDoctorGradeByQuestion(Doctor doctor);
        List<string> GetDoctorQuestions();
        List<PollQuestion> GetHospitalGradeByQuestion();
        List<string> GetHospitalQuestions();
        void LoadFromFile();
        void Save();
        void Update(int id, PollQuestion byPollQuestion);
    }
}