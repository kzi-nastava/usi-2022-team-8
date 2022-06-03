using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.GUI.PatientView;
using System.Windows;

namespace HealthInstitution.GUI.PatientWindows;

/// <summary>
/// Interaction logic for PatientScheduleWindow.xaml
/// </summary>
public partial class PatientScheduleWindow : Window
{
    private ExaminationRepository _examinationRepository = ExaminationRepository.GetInstance();
    private Patient _loggedPatient;

    public PatientScheduleWindow(Patient loggedPatient)
    {
        InitializeComponent();
        this._loggedPatient = loggedPatient;
        LoadRows();
    }

    private void GridRefresh()
    {
        dataGrid.Items.Clear();
        LoadRows();
    }

    private void AddButton_click(object sender, RoutedEventArgs e)
    {
        TrollCounterService.TrollCheck(_loggedPatient.Username);
        new AddExaminationDialog(_loggedPatient).ShowDialog();
        GridRefresh();
        TrollCounterService.AppendCreateDates(_loggedPatient.Username);
    }

    private void EditButton_click(object sender, RoutedEventArgs e)
    {
        TrollCounterService.TrollCheck(_loggedPatient.Username);
        Examination selectedExamination = (Examination)dataGrid.SelectedItem;
        new EditExaminationDialog(selectedExamination).ShowDialog();
        GridRefresh();
        TrollCounterService.AppendEditDeleteDates(_loggedPatient.Username);
    }

    private void DeleteButton_click(object sender, RoutedEventArgs e)
    {
        Examination selectedExamination = (Examination)dataGrid.SelectedItem;
        TrollCounterService.TrollCheck(_loggedPatient.Username);
        ExaminationDoctorRepository.GetInstance().Save();
        GridRefresh();
        TrollCounterService.AppendEditDeleteDates(_loggedPatient.Username);
        ConfirmDelete(selectedExamination);
    }

    private bool IsConfirmedDelete()
    {
        return System.Windows.MessageBox.Show("Are you sure you want to delete selected examination", "Question",
            MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
    }

    private void ConfirmDelete(Examination selectedExamination)
    {
        if (IsConfirmedDelete())
        {
            if (selectedExamination.Appointment.AddDays(-2) < DateTime.Now)
            {
                ScheduleEditRequestService.AddDeleteRequest(selectedExamination);
            }
            else
            {
                dataGrid.Items.Remove(selectedExamination);
                _examinationRepository.Delete(selectedExamination.Id);
                selectedExamination.Doctor.Examinations.Remove(selectedExamination);
            }
        }
    }

    private void LoadRows()
    {
        foreach (Examination examination in ExaminationRepository.GetInstance().Examinations)
        {
            if (examination.MedicalRecord.Patient.Username.Equals(_loggedPatient.Username))

                dataGrid.Items.Add(examination);
        }
        dataGrid.Items.Refresh();
    }
}