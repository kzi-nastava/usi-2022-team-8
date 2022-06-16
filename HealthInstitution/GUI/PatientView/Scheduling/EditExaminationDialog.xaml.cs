using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System.Windows;
using System.Windows.Controls;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;

namespace HealthInstitution.GUI.PatientWindows;

/// <summary>
/// Interaction logic for EditExaminationDialog.xaml
/// </summary>
///

public partial class EditExaminationDialog : Window
{
    private User _loggedPatient;
    private Examination _selectedExamination;
    private IDoctorService _doctorService;
    private IExaminationService _examinationService;
    private IEditSchedulingService _editSchedulingService;
    private IScheduleEditRequestsService _scheduleEditRequestService;

    public EditExaminationDialog(IDoctorService doctorService,
                                       IExaminationService examinationService,
                                         IEditSchedulingService editSchedulingService,
                                         IScheduleEditRequestsService scheduleEditRequestsService)
    {
        InitializeComponent();
        _doctorService = doctorService;
        _examinationService = examinationService;
        _editSchedulingService = editSchedulingService;
        _scheduleEditRequestService = scheduleEditRequestsService;
    }

    public void SetExamination(Examination examination)
    {
        _selectedExamination = examination;
        _loggedPatient = examination.MedicalRecord.Patient;
        DataContext = new EditExaminationDialogViewModel(this, examination, _doctorService, _examinationService, _editSchedulingService, _scheduleEditRequestService);
    }
}