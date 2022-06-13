using HealthInstitution.Core.Polls.Model;
using HealthInstitution.Core.Polls.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.GUI.ManagerView.PollView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.DoctorRatings;

namespace HealthInstitution.Core.Polls;

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
        filteredQuestions = filteredQuestions.Where(o => o.ForDoctor == pollQuestionDTO.ForDoctor).ToList();
        HandleAddingScores(filteredQuestions, pollQuestionDTO);
        UpdateDoctorRatings(pollQuestionDTO);
        s_pollQuestionRepository.Save();
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
            double avg = grades.Count > 0 ? Math.Round(grades.Average(), 2) : 0.0;
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
        var groups = grades.GroupBy(i => i);
        Dictionary<int, int> occurrenceByGrade = GetDefaultGrades();

        foreach (var grp in groups)
        {
            occurrenceByGrade[grp.Key] = grp.Count();
        }
        return occurrenceByGrade;
    }

    private static Dictionary<int, int> GetDefaultGrades()
    {
        Dictionary<int, int> occurrenceByGrade = new Dictionary<int, int>();
        for (int i = 1; i <= 5; i++)
        {
            occurrenceByGrade[i] = 0;
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