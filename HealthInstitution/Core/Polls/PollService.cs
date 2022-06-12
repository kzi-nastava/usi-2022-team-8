using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.Polls.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Polls
{
    static class PollService
    {
        static PollQuestionRepository s_pollQuestionRepository = PollQuestionRepository.GetInstance();
        static PollCommentRepository s_pollCommentRepository = PollCommentRepository.GetInstance();

        public static List<PollComment> GetAllComments()
        {
            return s_pollCommentRepository.GetAll();
        }

        public static PollComment GetCommentById(int id)
        {
            return s_pollCommentRepository.GetById(id);
        }

        public static void AddComment(PollCommentDTO pollCommentDTO)
        {
            PollComment pollComment = new PollComment(pollCommentDTO);
            s_pollCommentRepository.Add(pollComment);
        }

        public static void UpdateComment(int id, PollCommentDTO pollCommentDTO)
        {
            PollComment pollComment = new PollComment(pollCommentDTO);
            s_pollCommentRepository.Update(id, pollComment);
        }

        public static void DeleteComment(int id)
        {
            s_pollCommentRepository.Delete(id);
        }

        public static List<PollQuestion> GetAllQuestions()
        {
            return s_pollQuestionRepository.GetAll();
        }

        public static PollQuestion GetQuestionById(int id)
        {
            return s_pollQuestionRepository.GetById(id);
        }

        public static void AddQuestion(PollQuestionDTO pollQuestionDTO)
        {
            PollQuestion pollQuestion = new PollQuestion(pollQuestionDTO);
            s_pollQuestionRepository.Add(pollQuestion);
        }

        public static void UpdateQuestion(int id, PollQuestionDTO pollQuestionDTO)
        {
            PollQuestion pollQuestion = new PollQuestion(pollQuestionDTO);
            s_pollQuestionRepository.Update(id, pollQuestion);
        }

        public static void DeleteQuestion(int id)
        {
            s_pollQuestionRepository.Delete(id);
        }
    }
}
