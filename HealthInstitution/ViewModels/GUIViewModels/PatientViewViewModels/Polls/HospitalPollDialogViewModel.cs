using HealthInstitution.Commands.PatientCommands.PollCommands;
using HealthInstitution.Core.Polls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.Polls;

public class HospitalPollDialogViewModel : ViewModelBase
{
    private IPollService _pollService;

    public Window ThisWindow;

    public HospitalPollDialogViewModel(Window window, IPollService pollService)
    {
        ThisWindow = window;
        _pollService = pollService;
        SubmitCommand = new HospitalPollSubmitCommand(this, _pollService);
        LoadLabels();
    }

    private string _q1Text;

    public string Q1Text
    {
        get
        {
            return _q1Text;
        }
        set
        {
            _q1Text = value;
            OnPropertyChanged(nameof(Q1Text));
        }
    }

    private string _q2Text;

    public string Q2Text
    {
        get
        {
            return _q2Text;
        }
        set
        {
            _q2Text = value;
            OnPropertyChanged(nameof(Q2Text));
        }
    }

    private string _q3Text;

    public string Q3Text
    {
        get
        {
            return _q3Text;
        }
        set
        {
            _q3Text = value;
            OnPropertyChanged(nameof(Q3Text));
        }
    }

    private string _q4Text;

    public string Q4Text
    {
        get
        {
            return _q4Text;
        }
        set
        {
            _q4Text = value;
            OnPropertyChanged(nameof(Q4Text));
        }
    }

    private string _q5Text;

    public string Q5Text
    {
        get
        {
            return _q5Text;
        }
        set
        {
            _q5Text = value;
            OnPropertyChanged(nameof(Q5Text));
        }
    }

    private string _commentText;

    public string CommentText
    {
        get
        {
            return _commentText;
        }
        set
        {
            _commentText = value;
            OnPropertyChanged(nameof(CommentText));
        }
    }

    private object _q1Answer = "3";

    public object Q1Answer
    {
        get
        {
            return _q1Answer;
        }
        set
        {
            _q1Answer = value;
            OnPropertyChanged(nameof(Q1Answer));
        }
    }

    private object _q2Answer = "3";

    public object Q2Answer
    {
        get
        {
            return _q2Answer;
        }
        set
        {
            _q2Answer = value;
            OnPropertyChanged(nameof(Q2Answer));
        }
    }

    private object _q3Answer = "3";

    public object Q3Answer
    {
        get
        {
            return _q3Answer;
        }
        set
        {
            _q3Answer = value;
            OnPropertyChanged(nameof(Q3Answer));
        }
    }

    private object _q4Answer = "3";

    public object Q4Answer
    {
        get
        {
            return _q4Answer;
        }
        set
        {
            _q4Answer = value;
            OnPropertyChanged(nameof(Q4Answer));
        }
    }

    private object _q5Answer = "3";

    public object Q5Answer
    {
        get
        {
            return _q5Answer;
        }
        set
        {
            _q5Answer = value;
            OnPropertyChanged(nameof(Q5Answer));
        }
    }

    public ICommand SubmitCommand { get; }

    private void LoadLabels()
    {
        var questions = _pollService.GetHospitalQuestions();
        Q1Text = questions[0];
        Q2Text = questions[1];
        Q3Text = questions[2];
        Q4Text = questions[3];
        Q5Text = questions[4];
    }
}