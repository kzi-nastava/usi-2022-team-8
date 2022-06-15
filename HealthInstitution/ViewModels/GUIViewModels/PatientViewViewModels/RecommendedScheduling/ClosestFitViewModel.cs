using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthInstitution.Commands.PatientCommands.RecommendedSchedulingCommands;

namespace HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.RecommendedScheduling;

public class ClosestFitViewModel : ViewModelBase
{
    public List<Examination> Examinations;

    private ObservableCollection<ExaminationViewModel> _examinationVMs;

    public ObservableCollection<ExaminationViewModel> ExaminationVMs
    {
        get
        {
            return _examinationVMs;
        }
        set
        {
            _examinationVMs = value;
            OnPropertyChanged(nameof(ExaminationVMs));
        }
    }

    private object _choice;

    public object Choice
    {
        get
        {
            return _choice;
        }
        set
        {
            _choice = value;
            OnPropertyChanged(nameof(Choice));
        }
    }

    public Examination GetChosen()
    {
        return Examinations[Convert.ToInt32(Choice as string)];
    }

    public ICommand AddClosestFitExaminationCommand { get; }

    public ClosestFitViewModel(List<Examination> examinations)
    {
        Examinations = new();
        Examinations = examinations;
        _examinationVMs = new();
        AddClosestFitExaminationCommand = new ClosestFitCommand(this);
        LoadRows();
    }

    private void LoadRows()
    {
        foreach (Examination examination in Examinations)
            _examinationVMs.Add(new ExaminationViewModel(examination));
    }
}