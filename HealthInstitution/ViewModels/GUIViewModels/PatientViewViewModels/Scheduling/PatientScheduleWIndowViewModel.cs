using HealthInstitution.Commands.PatientCommands.Scheduling;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.TrollCounters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthInstitution.ViewModels.GUIViewModels.Scheduling;

public class PatientScheduleWindowViewModel : ViewModelBase
{
    public Patient LoggedPatient;

    public List<Examination> Examinations;

    private int _selectedExaminationIndex;

    private IExaminationService _examinationService;
    private IScheduleEditRequestsService _scheduleEditRequestsService;
    private ITrollCounterService _trollCounterService;

    public PatientScheduleWindowViewModel(Patient loggedPatient, IExaminationService examinationService, IScheduleEditRequestsService scheduleEditRequestsService,ITrollCounterService trollCounterService)
    {
        _examinationService = examinationService;
        _scheduleEditRequestsService = scheduleEditRequestsService;
        _trollCounterService = trollCounterService;
        LoggedPatient = loggedPatient;
        AddSchedulingCommand = new AddSchedulingCommand(this,loggedPatient,_trollCounterService);
        EditSchedulingCommand = new EditSchedulingCommand(this,_trollCounterService);
        DeleteSchedulingCommand = new DeleteSchedulingCommand(this,_examinationService,_trollCounterService,_scheduleEditRequestsService);
        Examinations = new();
        _examinationVMs = new();
        RefreshGrid();
    }

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

    public void RefreshGrid()
    {
        _examinationVMs.Clear();
        Examinations.Clear();
        foreach (Examination examination in _examinationService.GetAll())
        {
            if (examination.MedicalRecord.Patient.Username.Equals(LoggedPatient.Username))
            {
                Examinations.Add(examination);
                _examinationVMs.Add(new ExaminationViewModel(examination));
            }
        }
    }

    public Examination GetSelectedExamination()
    {
        return Examinations[_selectedExaminationIndex];
    }

    public ICommand AddSchedulingCommand { get; }
    public ICommand EditSchedulingCommand { get; }
    public ICommand DeleteSchedulingCommand { get; }

    
}