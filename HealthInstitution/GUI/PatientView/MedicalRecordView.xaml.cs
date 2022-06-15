using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System.Windows;
using HealthInstitution.GUI.PatientView.Polls;
using HealthInstitution.Core.Polls;
using HealthInstitution.Core.DIContainer;

namespace HealthInstitution.GUI.PatientView;

/// <summary>
/// Interaction logic for MedicalRecordView.xaml
/// </summary>
public partial class MedicalRecordView : Window
{
    private User _loggedPatient;
    private List<Examination> _currentExaminations;
    IExaminationService _examinationService;

    public MedicalRecordView(User loggedPatient, IExaminationService examinationService)
    {
        InitializeComponent();
        _loggedPatient = loggedPatient;
        _examinationService = examinationService;
        LoadAllRows();
    }

    private void DoctorButton_Click(object sender, RoutedEventArgs e)
    {
        LoadRows(_examinationService.OrderByDoctor(_currentExaminations));
    }

    private void DateButton_Click(object sender, RoutedEventArgs e)
    {
        LoadRows(_examinationService.OrderByDate(_currentExaminations));
    }

    private void SpecializationButton_Click(object sender, RoutedEventArgs e)
    {
        LoadRows(_examinationService.OrderByDoctorSpeciality(_currentExaminations));
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        LoadRows(_examinationService.GetSearchAnamnesis(searchParameter.Text, _loggedPatient.Username));
    }

    private void LoadAllRows()
    {
        LoadRows(_examinationService.GetCompletedByPatient(_loggedPatient.Username));
    }

    private void LoadRows(List<Examination> examinations)
    {
        _currentExaminations = examinations;
        dataGrid.Items.Clear();
        foreach (Examination examination in examinations)
        {
            if (examination.MedicalRecord.Patient.Username.Equals(_loggedPatient.Username))
                dataGrid.Items.Add(examination);
        }
        dataGrid.Items.Refresh();
    }

    private void SearchParameter_GotFocus(object sender, RoutedEventArgs e)
    {
        searchParameter.Clear();
    }

    private void RateDoctorButton_Click(object sender, RoutedEventArgs e)
    {
        Examination selectedExamination = (Examination)dataGrid.SelectedItem;
        new DoctorPollDialog(selectedExamination.Doctor, DIContainer.GetService<IPollService>()).ShowDialog();
    }
}