using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.Polls.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.DoctorRatings;

namespace HealthInstitution.Core.Polls
{
    internal static class PollService
    {
        private static PollQuestionRepository s_pollQuestionRepository = PollQuestionRepository.GetInstance();
        private static PollCommentRepository s_pollCommentRepository = PollCommentRepository.GetInstance();

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

        public static List<string> GetHospitalQuestions()
        {
            return s_pollQuestionRepository.GetHospitalQuestions();
        }

        public static List<string> GetDoctorQuestions()
        {
            return s_pollQuestionRepository.GetDoctorQuestions();
        }

        public static void UpdateQuestionGrades(PollQuestionDTO pollQuestionDTO)
        {
            List<PollQuestion> allQuestions = GetAllQuestions();
            List<PollQuestion> filteredQuestions = allQuestions.Where(o => o.Question == pollQuestionDTO.Question).ToList();
            filteredQuestions = filteredQuestions.Where(o => o.ForDoctor == null).ToList();
            HandleAddingScores(filteredQuestions, pollQuestionDTO);
            UpdateDoctorRatings(pollQuestionDTO);
            s_pollQuestionRepository.Save();
        }

        private static void UpdateDoctorRatings(PollQuestionDTO pollQuestionDTO)
        {
            if (pollQuestionDTO.ForDoctor != null)
            {
                DoctorRatingsService.UpdateScore(pollQuestionDTO.ForDoctor.Username, pollQuestionDTO.Grades[0]);
            }
        }

        private static void HandleAddingScores(List<PollQuestion> filteredQuestions, PollQuestionDTO pollQuestionDTO)
        {
            if (filteredQuestions.Count > 0)
                filteredQuestions[0].Grades.AddRange(pollQuestionDTO.Grades);
            else
                AddQuestion(pollQuestionDTO);
        }
    }
}