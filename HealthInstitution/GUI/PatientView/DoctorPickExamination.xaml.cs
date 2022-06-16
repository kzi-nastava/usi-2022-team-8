using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System.Windows;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for DoctorPickExamination.xaml
/// </summary>
public partial class DoctorPickExamination : Window
{
    private Patient _loggedPatient;
    private List<Doctor> _currentDoctors;
    IDoctorService _doctorService;

    public DoctorPickExamination(IDoctorService doctorService)
    {
        InitializeComponent();
        dataGrid.SelectedIndex = 0;
        _doctorService = doctorService;
        _currentDoctors = _doctorService.GetAll();
        
    }
    public void SetLoggedPatient(Patient patient)
    {
        _loggedPatient = patient;
        //DataContext = new DoctorPickExaminationViewModel();
        
    }
}