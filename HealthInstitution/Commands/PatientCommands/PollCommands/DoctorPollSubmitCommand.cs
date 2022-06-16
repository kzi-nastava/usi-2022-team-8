using HealthInstitution.Core;
using HealthInstitution.Core.Polls;
using HealthInstitution.Core.Polls.Model;
using HealthInstitution.ViewModels.GUIViewModels.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.PollCommands;

public class DoctorPollSubmitCommand : CommandBase
{
    private DoctorPollViewModel _doctorPollViewModel;
    IPollService _pollService;
    public DoctorPollSubmitCommand(DoctorPollViewModel doctorPollViewModel,IPollService pollService)
    {
        _doctorPollViewModel = doctorPollViewModel;
        _pollService = pollService;
    }

    public override void Execute(object? parameter)
    {
        AddComment();
        AddForOneQuestion(_doctorPollViewModel.Q1Text, _doctorPollViewModel.Q1Answer);
        AddForOneQuestion(_doctorPollViewModel.Q2Text, _doctorPollViewModel.Q2Answer);
        AddForOneQuestion(_doctorPollViewModel.Q3Text, _doctorPollViewModel.Q3Answer);
        AddForOneQuestion(_doctorPollViewModel.Q4Text, _doctorPollViewModel.Q4Answer);
        AddForOneQuestion(_doctorPollViewModel.Q5Text, _doctorPollViewModel.Q5Answer);
    }

    private void AddForOneQuestion(string question, object answer)
    {
        List<int> ints = new List<int>();
        ints.Add(Convert.ToInt32(answer as string));

        PollQuestionDTO pollQuestionDTO = new PollQuestionDTO(question, _doctorPollViewModel.Doctor, ints);
        _pollService.UpdateQuestionGrades(pollQuestionDTO);
    }

    private void AddComment()
    {
        string comment = _doctorPollViewModel.CommentText;
        if (comment == null) return;
        if (comment.Length > 0)
        {
            comment = comment.Trim();
            PollCommentDTO pollCommentDTO = new PollCommentDTO(comment, _doctorPollViewModel.Doctor);
            _pollService.AddComment(pollCommentDTO);
        }
    }
}