using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System.Windows;
using HealthInstitution.GUI.PatientView.Polls;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for MedicalRecordView.xaml
/// </summary>
public partial class MedicalRecordView : Window
{
    private User _loggedPatient;

    public MedicalRecordView(User loggedPatient)
    {
        InitializeComponent();
        _loggedPatient = loggedPatient;
    }

    private void RateDoctorButton_Click(object sender, RoutedEventArgs e)
    {
        Examination selectedExamination = (Examination)dataGrid.SelectedItem;
        new DoctorPollDialog(selectedExamination.Doctor).ShowDialog();
    }
}