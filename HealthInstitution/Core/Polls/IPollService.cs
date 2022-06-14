using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.GUI.ManagerView.PollView;

namespace HealthInstitution.Core.Polls
{
    public interface IPollService
    {
        void AddComment(PollCommentDTO pollCommentDTO);
        void AddQuestion(PollQuestionDTO pollQuestionDTO);
        void DeleteComment(int id);
        void DeleteQuestion(int id);
        List<PollComment> GetAllComments();
        List<PollQuestion> GetAllQuestions();
        PollComment GetCommentById(int id);
        List<PollComment> GetCommentsByDoctor(Doctor doctor);
        List<TableItemPoll> GetDoctorPollByQuestions(Doctor doctor);
        List<string> GetDoctorQuestions();
        List<PollComment> GetHospitalComments();
        List<TableItemPoll> GetHospitalPollByQuestions();
        List<string> GetHospitalQuestions();
        PollQuestion GetQuestionById(int id);
        void UpdateComment(int id, PollCommentDTO pollCommentDTO);
        void UpdateQuestion(int id, PollQuestionDTO pollQuestionDTO);
        void UpdateQuestionGrades(PollQuestionDTO pollQuestionDTO);
    }
}