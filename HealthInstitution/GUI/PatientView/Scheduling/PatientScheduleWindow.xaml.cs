using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
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
    private Patient _loggedPatient;
    IExaminationService _examinationService;
    ITrollCounterService _trollCounterService;
    IScheduleEditRequestsService _scheduleEditRequestService;


    public PatientScheduleWindow(Patient loggedPatient, IExaminationService examinationService,
                                 ITrollCounterService trollCounterService,
                                 IScheduleEditRequestsService scheduleEditRequestService)
    {
        InitializeComponent();
        this._loggedPatient = loggedPatient;
        this._examinationService = examinationService;
        this._trollCounterService = trollCounterService;
        _scheduleEditRequestService = scheduleEditRequestService;
        LoadRows();
    }

    private void GridRefresh()
    {
        dataGrid.Items.Clear();
        LoadRows();
    }

    private void AddButton_click(object sender, RoutedEventArgs e)
    {
        _trollCounterService.TrollCheck(_loggedPatient.Username);
        new AddExaminationDialog(_loggedPatient, DIContainer.GetService<IDoctorService>(), DIContainer.GetService<IMedicalRecordService>(), DIContainer.GetService<ISchedulingService>()).ShowDialog();
        GridRefresh();
        _trollCounterService.AppendCreateDates(_loggedPatient.Username);
    }

    private void EditButton_click(object sender, RoutedEventArgs e)
    {
        _trollCounterService.TrollCheck(_loggedPatient.Username);
        Examination selectedExamination = (Examination)dataGrid.SelectedItem;
        new EditExaminationDialog(selectedExamination, DIContainer.GetService<IDoctorService>(), DIContainer.GetService<IExaminationService>(), DIContainer.GetService<IEditSchedulingService>(), DIContainer.GetService<IScheduleEditRequestsService>()).ShowDialog();
        GridRefresh();
        _trollCounterService.AppendEditDeleteDates(_loggedPatient.Username);
    }

    private void DeleteButton_click(object sender, RoutedEventArgs e)
    {
        Examination selectedExamination = (Examination)dataGrid.SelectedItem;
        _trollCounterService.TrollCheck(_loggedPatient.Username);
        GridRefresh();
        _trollCounterService.AppendEditDeleteDates(_loggedPatient.Username);
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
                _scheduleEditRequestService.AddDeleteRequest(selectedExamination);
            }
            else
            {
                dataGrid.Items.Remove(selectedExamination);
                _examinationService.Delete(selectedExamination.Id);
                selectedExamination.Doctor.Examinations.Remove(selectedExamination);
            }
        }
    }

    private void LoadRows()
    {
        foreach (Examination examination in _examinationService.GetAll())
        {
            if (examination.MedicalRecord.Patient.Username.Equals(_loggedPatient.Username))

                dataGrid.Items.Add(examination);
        }
        dataGrid.Items.Refresh();
    }
}