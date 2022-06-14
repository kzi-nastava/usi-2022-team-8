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
    private ObservableCollection<ExaminationViewModel> _examinations;

    public ObservableCollection<ExaminationViewModel> CompletedExaminations
    {
        get
        {
            return _examinations;
        }
        set
        {
            _examinations = value;
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

    private User _loggedPatient;

    public MedicalRecordViewModel(User patient)
    {
        this._examinations = new();
        _loggedPatient = patient;
        foreach (Examination examination in ExaminationService.GetCompletedByPatient(_loggedPatient.Username))
        {
            var vm = new ExaminationViewModel(examination);
            _examinations.Add(vm);
        }
    }
}