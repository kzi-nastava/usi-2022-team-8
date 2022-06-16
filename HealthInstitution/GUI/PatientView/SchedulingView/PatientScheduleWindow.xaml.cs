using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.GUI.PatientView;
using System.Windows;

using HealthInstitution.Core.Examinations;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;

namespace HealthInstitution.GUI.PatientWindows;

/// <summary>
/// Interaction logic for PatientScheduleWindow.xaml
/// </summary>
public partial class PatientScheduleWindow : Window
{
    private Patient _loggedPatient;
    IExaminationService _examinationService;
    ITrollCounterService _trollCounterService;
    IScheduleEditRequestsService _scheduleEditRequestService;


    public PatientScheduleWindow(IExaminationService examinationService,
                                 ITrollCounterService trollCounterService,
                                 IScheduleEditRequestsService scheduleEditRequestService)
    {
        InitializeComponent();
        this._examinationService = examinationService;
        this._trollCounterService = trollCounterService;
        _scheduleEditRequestService = scheduleEditRequestService;
        
    }
    public void SetLoggedPatient(Patient patient)
    {
        _loggedPatient = patient;
        DataContext = new PatientScheduleWindowViewModel(patient, _examinationService, _scheduleEditRequestService, _trollCounterService);
    }
}