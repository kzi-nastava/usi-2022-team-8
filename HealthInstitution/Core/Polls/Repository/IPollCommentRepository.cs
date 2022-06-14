using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;

namespace HealthInstitution.Core.Polls.Repository
{
    public interface IPollCommentRepository
    {
        void Add(PollComment pollComment);
        void Delete(int id);
        List<PollComment> GetAll();
        Dictionary<int, PollComment> GetAllById();
        PollComment GetById(int id);
        List<PollComment> GetCommentsByDoctor(Doctor doctor);
        List<PollComment> GetHospitalComments();
        void LoadFromFile();
        void Save();
        void Update(int id, PollComment byPollComment);
    }
}