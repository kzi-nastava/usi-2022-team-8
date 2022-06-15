using System.Windows;
using System.Windows.Controls;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.PrescriptionNotifications.Model;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.SystemUsers.Patients;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for RecepieNotificationSettings.xaml
/// </summary>
public partial class RecepieNotificationSettingsDialog : Window
{
    private int _hours;
    private int _minutes;
    private string _loggedPatinet;
    private List<Prescription> _prescriptions;

    public RecepieNotificationSettingsDialog(string loggedPatient)
    {
        InitializeComponent();
    }
}