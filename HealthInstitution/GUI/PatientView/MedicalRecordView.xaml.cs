using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System.Windows;
using HealthInstitution.GUI.PatientView.Polls;
using HealthInstitution.Core.Polls;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for MedicalRecordView.xaml
/// </summary>
public partial class MedicalRecordView : Window
{
    private User _loggedPatient;

    public MedicalRecordView(IExaminationService examinationService)
    {
        InitializeComponent();
        _loggedPatient = loggedPatient;
    }

    private void RateDoctorButton_Click(object sender, RoutedEventArgs e)
    {
        Examination selectedExamination = (Examination)dataGrid.SelectedItem;

        DoctorPollDialog doctorPollDialog = DIContainer.GetService<DoctorPollDialog>();
        doctorPollDialog.SetRatedDoctor(selectedExamination.Doctor);
        doctorPollDialog.ShowDialog();
    }
}