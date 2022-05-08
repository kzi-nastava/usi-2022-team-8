using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.TrollCounters.Repository;
using HealthInstitution.GUI.PatientView;
using System.Windows;

namespace HealthInstitution.GUI.PatientWindows;

/// <summary>
/// Interaction logic for PatientScheduleWindow.xaml
/// </summary>
public partial class PatientScheduleWindow : Window
{
    private ExaminationRepository _examinationRepository = ExaminationRepository.GetInstance();
    private User _loggedPatient;

    public PatientScheduleWindow(User loggedPatient)
    {
        InitializeComponent();
        this._loggedPatient = loggedPatient;
        loadRows();
    }

    private void addButton_click(object sender, RoutedEventArgs e)
    {
        try
        {
            TrollCounterFileRepository.GetInstance().TrollCheck(_loggedPatient.Username);
            AddExaminationDialog addExaminationDialog = new AddExaminationDialog(_loggedPatient);
            addExaminationDialog.ShowDialog();
            dataGrid.Items.Clear();
            loadRows();
            TrollCounterFileRepository.GetInstance().GetById(_loggedPatient.Username).AppendCreateDates(DateTime.Today);
            TrollCounterFileRepository.GetInstance().Save();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Troll Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }

    private void editButton_click(object sender, RoutedEventArgs e)
    {
        try
        {
            TrollCounterFileRepository.GetInstance().TrollCheck(_loggedPatient.Username);
            Examination selectedExamination = (Examination)dataGrid.SelectedItem;
            EditExaminationDialog editExaminationDialog = new EditExaminationDialog(selectedExamination);
            editExaminationDialog.ShowDialog();
            dataGrid.Items.Clear();
            loadRows();
            TrollCounterFileRepository.GetInstance().GetById(_loggedPatient.Username).AppendEditDeleteDates(DateTime.Today);
            TrollCounterFileRepository.GetInstance().Save();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Troll Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }

    private void deleteButton_click(object sender, RoutedEventArgs e)
    {
        Examination selectedExamination = (Examination)dataGrid.SelectedItem;
        try
        {
            TrollCounterFileRepository.GetInstance().TrollCheck(_loggedPatient.Username);
            ExaminationDoctorRepository.GetInstance().Save();
            dataGrid.Items.Clear();
            loadRows();
            TrollCounterFileRepository.GetInstance().GetById(_loggedPatient.Username).AppendEditDeleteDates(DateTime.Today);
            TrollCounterFileRepository.GetInstance().Save();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Troll Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
        if (System.Windows.MessageBox.Show("Are you sure you want to delete selected examination", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            if (selectedExamination.Appointment.AddDays(-2) < DateTime.Now)
            {
                ScheduleEditRequestFileRepository.GetInstance().AddDeleteRequest(selectedExamination);
            }
            else
            {
                dataGrid.Items.Remove(selectedExamination);
                _examinationRepository.DeleteExamination(selectedExamination.Id);
                selectedExamination.Doctor.Examinations.Remove(selectedExamination);
            }
        }
    }

    /*private void dataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        LoadGrid();
    }*/

    private void loadRows()
    {
        foreach (Examination examination in ExaminationRepository.GetInstance().Examinations)
        {
            if (examination.MedicalRecord.Patient.Username.Equals(_loggedPatient.Username))

                dataGrid.Items.Add(examination);
        }
        dataGrid.Items.Refresh();
    }
}