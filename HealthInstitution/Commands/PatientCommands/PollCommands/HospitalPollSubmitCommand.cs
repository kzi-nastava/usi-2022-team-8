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

public class HospitalPollSubmitCommand : CommandBase
{
    private HospitalPollDialogViewModel _hospitalPollDialogViewModel;
    private IPollService _pollService;

    public HospitalPollSubmitCommand(HospitalPollDialogViewModel hospitalPollDialogViewModel, IPollService pollService)
    {
        _hospitalPollDialogViewModel = hospitalPollDialogViewModel;
        _pollService = pollService;
    }

    public override void Execute(object? parameter)
    {
        AddComment();
        AddForOneQuestion(_hospitalPollDialogViewModel.Q1Text, _hospitalPollDialogViewModel.Q1Answer);
        AddForOneQuestion(_hospitalPollDialogViewModel.Q2Text, _hospitalPollDialogViewModel.Q2Answer);
        AddForOneQuestion(_hospitalPollDialogViewModel.Q3Text, _hospitalPollDialogViewModel.Q3Answer);
        AddForOneQuestion(_hospitalPollDialogViewModel.Q4Text, _hospitalPollDialogViewModel.Q4Answer);
        AddForOneQuestion(_hospitalPollDialogViewModel.Q5Text, _hospitalPollDialogViewModel.Q5Answer);
        _hospitalPollDialogViewModel.ThisWindow.Close();
    }

    private void AddForOneQuestion(string question, object answer)
    {
        List<int> ints = new List<int>();
        ints.Add(Convert.ToInt32(answer as string));

        PollQuestionDTO pollQuestionDTO = new PollQuestionDTO(question, null, ints);
        _pollService.UpdateQuestionGrades(pollQuestionDTO);
    }

    private void AddComment()
    {
        string comment = _hospitalPollDialogViewModel.CommentText;
        if (comment == null) return;
        if (comment.Length > 0)
        {
            comment = comment.Trim();
            PollCommentDTO pollCommentDTO = new PollCommentDTO(comment, null);
            _pollService.AddComment(pollCommentDTO);
        }
    }
}