using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.Scheduling;
using System.Windows;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for DoctorPickExamination.xaml
/// </summary>
public partial class DoctorPickExamination : Window
{
    private Patient _loggedPatient;
    private List<Doctor> _currentDoctors;
    private IDoctorService _doctorService;
    private ITrollCounterService _trollCounterService;

    public DoctorPickExamination(IDoctorService doctorService, ITrollCounterService trollCounterService)
    {
        InitializeComponent();
        dataGrid.SelectedIndex = 0;
        _doctorService = doctorService;
        _trollCounterService = trollCounterService;
        _currentDoctors = _doctorService.GetAll();
    }

    public void SetLoggedPatient(Patient patient)
    {
        _loggedPatient = patient;
        DataContext = new DoctorPickViewModel(_loggedPatient, _doctorService, _trollCounterService);
    }
}