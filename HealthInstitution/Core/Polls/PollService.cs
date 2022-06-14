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

public class PollService : IPollService
{
    IPollQuestionRepository _pollQuestionRepository;
    IPollCommentRepository _pollCommentRepository;
    IDoctorRatingsService _doctorRatingService;

    public PollService(IPollQuestionRepository pollQuestionRepository, IPollCommentRepository pollCommentRepository, IDoctorRatingsService doctorRatingService)
    {
        _pollQuestionRepository = pollQuestionRepository;
        _pollCommentRepository = pollCommentRepository;
        _doctorRatingService = doctorRatingService;
    }

    public List<PollComment> GetAllComments()
    {
        return _pollCommentRepository.GetAll();
    }

    public PollComment GetCommentById(int id)
    {
        return _pollCommentRepository.GetById(id);
    }

    public void AddComment(PollCommentDTO pollCommentDTO)
    {
        PollComment pollComment = new PollComment(pollCommentDTO);
        _pollCommentRepository.Add(pollComment);
    }

    public void UpdateComment(int id, PollCommentDTO pollCommentDTO)
    {
        PollComment pollComment = new PollComment(pollCommentDTO);
        _pollCommentRepository.Update(id, pollComment);
    }

    public void DeleteComment(int id)
    {
        _pollCommentRepository.Delete(id);
    }

    public List<PollQuestion> GetAllQuestions()
    {
        return _pollQuestionRepository.GetAll();
    }

    public PollQuestion GetQuestionById(int id)
    {
        return _pollQuestionRepository.GetById(id);
    }

    public void AddQuestion(PollQuestionDTO pollQuestionDTO)
    {
        PollQuestion pollQuestion = new PollQuestion(pollQuestionDTO);
        _pollQuestionRepository.Add(pollQuestion);
    }

    public void UpdateQuestion(int id, PollQuestionDTO pollQuestionDTO)
    {
        PollQuestion pollQuestion = new PollQuestion(pollQuestionDTO);
        _pollQuestionRepository.Update(id, pollQuestion);
    }

    public void DeleteQuestion(int id)
    {
        _pollQuestionRepository.Delete(id);
    }

    public List<string> GetHospitalQuestions()
    {
        return _pollQuestionRepository.GetHospitalQuestions();
    }

    public List<string> GetDoctorQuestions()
    {
        return _pollQuestionRepository.GetDoctorQuestions();
    }

    public void UpdateQuestionGrades(PollQuestionDTO pollQuestionDTO)
    {
        List<PollQuestion> allQuestions = GetAllQuestions();
        List<PollQuestion> filteredQuestions = allQuestions.Where(o => o.Question == pollQuestionDTO.Question).ToList();
        filteredQuestions = filteredQuestions.Where(o => o.ForDoctor == pollQuestionDTO.ForDoctor).ToList();
        HandleAddingScores(filteredQuestions, pollQuestionDTO);
        UpdateDoctorRatings(pollQuestionDTO);
        _pollQuestionRepository.Save();
    }

    public List<TableItemPoll> GetHospitalPollByQuestions()
    {
        List<PollQuestion> hospitalQuestions = _pollQuestionRepository.GetHospitalGradeByQuestion();
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

    public List<TableItemPoll> GetDoctorPollByQuestions(Doctor doctor)
    {
        List<PollQuestion> hospitalQuestions = _pollQuestionRepository.GetDoctorGradeByQuestion(doctor);
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

    private Dictionary<int, int> GetOccurrenceByGrade(List<int> grades)
    {
        var groups = grades.GroupBy(i => i);
        Dictionary<int, int> occurrenceByGrade = GetDefaultGrades();

        foreach (var grp in groups)
        {
            occurrenceByGrade[grp.Key] = grp.Count();
        }
        return occurrenceByGrade;
    }

    private Dictionary<int, int> GetDefaultGrades()
    {
        Dictionary<int, int> occurrenceByGrade = new Dictionary<int, int>();
        for (int i = 1; i <= 5; i++)
        {
            occurrenceByGrade[i] = 0;
        }
        return occurrenceByGrade;
    }

    public List<PollComment> GetHospitalComments()
    {
        return _pollCommentRepository.GetHospitalComments();
    }

    public List<PollComment> GetCommentsByDoctor(Doctor doctor)
    {
        return _pollCommentRepository.GetCommentsByDoctor(doctor);
    }

    private void UpdateDoctorRatings(PollQuestionDTO pollQuestionDTO)
    {
        if (pollQuestionDTO.ForDoctor != null)
        {
            _doctorRatingsService.UpdateScore(pollQuestionDTO.ForDoctor.Username, pollQuestionDTO.Grades[0]);
        }
    }

    private void HandleAddingScores(List<PollQuestion> filteredQuestions, PollQuestionDTO pollQuestionDTO)
    {
        if (filteredQuestions.Count > 0)
            filteredQuestions[0].Grades.AddRange(pollQuestionDTO.Grades);
        else
            AddQuestion(pollQuestionDTO);
    }
}