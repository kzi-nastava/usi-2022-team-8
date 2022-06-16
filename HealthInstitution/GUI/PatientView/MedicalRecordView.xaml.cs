using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System.Windows;
using HealthInstitution.GUI.PatientView.Polls;
using HealthInstitution.Core.Polls;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;
using HealthInstitution.Core.SystemUsers.Patients.Model;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for MedicalRecordView.xaml
/// </summary>
public partial class MedicalRecordView : Window
{
    private User _loggedPatient;
    IExaminationService _examinationService;
    IPollService _pollService;
    public MedicalRecordView(IExaminationService examinationService, IPollService pollService)
    {
        InitializeComponent();
        _examinationService = examinationService;
        _pollService = pollService;
    }

    public void SetLoggedPatient(User patient)
    {
        _loggedPatient = patient;
        DataContext = new MedicalRecordViewViewModel(patient, _examinationService, _pollService);
    }
}