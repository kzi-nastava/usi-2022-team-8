using HealthInstitution.Commands.Patient;
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

namespace HealthInstitution.GUI.PatientViewModel;

public class MedicalRecordViewModel : ViewModelBase
{
    //TODO all other commands
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

    public ICommand SearchKeywordCommand { get; }
    public ICommand RateDoctorCommand { get; }
    public ICommand DoctorSortCommand { get; }
    public ICommand DateSortCommand { get; }
    public ICommand SpecializationSortCommand { get; }

    public User LoggedPatient { get; set; }

    public MedicalRecordViewModel(User patient)
    {
        this._examinationVMs = new();
        LoggedPatient = patient;
        this.Examinations = ExaminationService.GetCompletedByPatient(LoggedPatient.Username);
        PutIntoGrid();
        DoctorSortCommand = new DoctorSortCommand(this);
        SearchKeywordCommand = new SearchAnamnesisCommand(this);
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