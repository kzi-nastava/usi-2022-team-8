using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HealthInstitution.Core.PrescriptionNotifications.Model;
using HealthInstitution.Core.PrescriptionNotifications.Service;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.PrescriptionNotificationViewModels;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for RecepieNotificationDialog.xaml
/// </summary>
public partial class RecepieNotificationDialog : Window
{
    IPrescriptionNotificationService _prescriptionNotificationService;
    Patient _loggedPatient;
    public RecepieNotificationDialog(IPrescriptionNotificationService prescriptionNotificationService)
    {
        InitializeComponent();
        _prescriptionNotificationService = prescriptionNotificationService;
    }

    public void SetLoggedPatient(Patient loggedPatient)
    {
        _loggedPatient = loggedPatient;
        DataContext = new PrescriptionNotificationDialogViewModel(loggedPatient, _prescriptionNotificationService);
    }

}