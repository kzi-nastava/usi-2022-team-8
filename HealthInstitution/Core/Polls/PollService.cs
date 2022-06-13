using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.Polls.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.GUI.ManagerView.PollView;
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

        public static List<string> GetHospitalQuestions()
        {
            return s_pollQuestionRepository.GetHospitalQuestions();
        }

        public static List<string> GetDoctorQuestions()
        {
            return s_pollQuestionRepository.GetDoctorQuestions();
        }

        public static List<TableItemPoll> GetHospitalPollByQuestions()
        {
            List<PollQuestion> hospitalQuestions = s_pollQuestionRepository.GetHospitalGradeByQuestion();
            List<TableItemPoll> items = new List<TableItemPoll>();

            var groupByQuestion = hospitalQuestions.ToLookup(q => q.Question);
            foreach (var group in groupByQuestion)
            {
                List<int> grades = group.SelectMany(q => q.Grades).ToList();
                var occurrenceByGrade = GetOccurrenceByGrade(grades);
                double avg = grades.Count > 0 ? Math.Round(grades.Average(),2) : 0.0;
                items.Add(new TableItemPoll(group.Key, avg, occurrenceByGrade));
            }

            return items;
        }

        public static List<TableItemPoll> GetDoctorPollByQuestions(Doctor doctor)
        {
            List<PollQuestion> hospitalQuestions = s_pollQuestionRepository.GetDoctorGradeByQuestion(doctor);
            List<TableItemPoll> items = new List<TableItemPoll>();

            var groupByQuestion = hospitalQuestions.ToLookup(q => q.Question);
            foreach (var group in groupByQuestion)
            {
                List<int> grades = group.SelectMany(q => q.Grades).ToList();
                var occurrenceByGrade = GetOccurrenceByGrade(grades);
                double avg = grades.Count > 0 ? Math.Round(grades.Average(), 2) : 0.0;
                items.Add(new TableItemPoll(group.Key, avg, occurrenceByGrade));
            }

            return items;
        }

        private static Dictionary<int, int> GetOccurrenceByGrade(List<int> grades)
        {
            Dictionary<int,int> occurrenceByGrade = new Dictionary<int,int>();
            var groups = grades.GroupBy(i => i);

            foreach (var grp in groups)
            {
                occurrenceByGrade[grp.Key] = grp.Count();
            }
            return occurrenceByGrade;
        }

        public static List<PollComment> GetHospitalComments()
        {
            return s_pollCommentRepository.GetHospitalComments();
        }

        public static List<PollComment> GetCommentsByDoctor(Doctor doctor)
        {
            return s_pollCommentRepository.GetCommentsByDoctor(doctor);
        }
    }
}
