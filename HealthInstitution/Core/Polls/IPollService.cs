using HealthInstitution.Core.Polls.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Polls
{
    public interface IPollService
    {
        public List<PollComment> GetAllComments();
        public PollComment GetCommentById(int id);
        public void AddComment(PollCommentDTO pollCommentDTO);
        public void UpdateComment(int id, PollCommentDTO pollCommentDTO);
        public void DeleteComment(int id);
        public List<PollQuestion> GetAllQuestions();
        public PollQuestion GetQuestionById(int id);
        public void AddQuestion(PollQuestionDTO pollQuestionDTO);
        public void UpdateQuestion(int id, PollQuestionDTO pollQuestionDTO);
        public void DeleteQuestion(int id);
    }
}
