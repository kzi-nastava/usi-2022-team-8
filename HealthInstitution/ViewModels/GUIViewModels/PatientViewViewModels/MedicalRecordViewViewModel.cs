using HealthInstitution.Commands.PatientCommands;
using HealthInstitution.Commands.PatientCommands.MedicalRecordViewCommands;
using HealthInstitution.Core;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;

public class MedicalRecordViewViewModel : ViewModelBase
{
    public ICommand SearchKeywordCommand { get; }
    public ICommand RateDoctorCommand { get; }
    public ICommand DoctorSortCommand { get; }
    public ICommand DateSortCommand { get; }
    public ICommand SpecializationSortCommand { get; }

    public User LoggedPatient { get; set; }
    IExaminationService _examinationService;

    public MedicalRecordViewViewModel(User patient, IExaminationService examinationService)
    {
        _examinationVMs = new();
        LoggedPatient = patient;
        _examinationService = examinationService;

        Examinations = ExaminationService.GetCompletedByPatient(LoggedPatient.Username);
        PutIntoGrid();
        DoctorSortCommand = new DoctorSortCommand(this);
        SearchKeywordCommand = new SearchAnamnesisCommand(this);
        SpecializationSortCommand = new SpecializationSortCommand(this);
        DateSortCommand = new DateSortCommand(this);
        RateDoctorCommand = new RateDoctorCommand(this);
    }
    public List<Examination> Examinations { get; set; }

    private ObservableCollection<ExaminationViewModel> _examinationVMs;

    public ObservableCollection<ExaminationViewModel> CompletedExaminations
    {
        get
        {
            return _examinationVMs;
        }
        set
        {
            _examinationVMs = value;
            OnPropertyChanged(nameof(CompletedExaminations));
        }
    }

    private string _keyword;

    public string Keyword
    {
        get
        {
            return _keyword;
        }
        set
        {
            _keyword = value;
            OnPropertyChanged(nameof(Keyword));
        }
    }

    private int _selectedExaminationIndex;

    public int SelectedExaminationIndex
    {
        get
        {
            return _selectedExaminationIndex;
        }
        set
        {
            _selectedExaminationIndex = value;
            OnPropertyChanged(nameof(SelectedExaminationIndex));
        }
    }

    

    public void ClosingHandle()
    {
        throw new NotImplementedException();
    }

    public void PutIntoGrid()
    {
        _examinationVMs.Clear();
        foreach (Examination examination in Examinations)
        {
            var vm = new ExaminationViewModel(examination);
            _examinationVMs.Add(vm);
        }
    }
}