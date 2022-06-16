using System.Windows;
using System.Windows.Controls;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.PrescriptionNotifications.Model;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.PrescriptionNotificationViewModels;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for RecepieNotificationSettings.xaml
/// </summary>
public partial class RecepieNotificationSettingsDialog : Window
{
    private string _loggedPatinet;
    IMedicalRecordService _medicalRecordService;
    IPatientService _patientService;
    IPrescriptionNotificationService _prescriptionNotificationService;
    public RecepieNotificationSettingsDialog(IMedicalRecordService medicalRecordService,
        IPatientService patientService, IPrescriptionNotificationService prescriptionNotificationService)
    {
        InitializeComponent();
        _medicalRecordService = medicalRecordService;   
        _patientService = patientService;
        _prescriptionNotificationService = prescriptionNotificationService;
        
    }
    public void SetLoggedPatient(string patient)
    {
        _loggedPatinet=patient;
        DataContext = new PrescriptionNotificationSettingViewModel(_patientService.GetByUsername(patient), _medicalRecordService, _patientService, _prescriptionNotificationService);
    }
}