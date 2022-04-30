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
    private ExaminationRepository examinationRepository = ExaminationRepository.GetInstance();
    private User loggedPatient;

    public PatientScheduleWindow(User loggedPatient)
    {
        InitializeComponent();
        this.loggedPatient = loggedPatient;
    }

    private void addButton_click(object sender, RoutedEventArgs e)
    {
        try
        {
            TrollCounterFileRepository.GetInstance().TrollCheck(loggedPatient.username);
            AddExaminationDialog addExaminationDialog = new AddExaminationDialog(loggedPatient);
            addExaminationDialog.ShowDialog();
            dataGrid.Items.Clear();
            LoadGrid();
            TrollCounterFileRepository.GetInstance().GetTrollCounterById(loggedPatient.username).AppendCreateDates(DateTime.Today);
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
            TrollCounterFileRepository.GetInstance().TrollCheck(loggedPatient.username);
            Examination selectedExamination = (Examination)dataGrid.SelectedItem;
            EditExaminationDialog editExaminationDialog = new EditExaminationDialog(selectedExamination);
            editExaminationDialog.ShowDialog();
            dataGrid.Items.Clear();
            LoadGrid();
            TrollCounterFileRepository.GetInstance().GetTrollCounterById(loggedPatient.username).AppendEditDeleteDates(DateTime.Today);
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
            TrollCounterFileRepository.GetInstance().TrollCheck(loggedPatient.username);
            ExaminationDoctorRepository.GetInstance().SaveToFile();
            dataGrid.Items.Clear();
            LoadGrid();
            TrollCounterFileRepository.GetInstance().GetTrollCounterById(loggedPatient.username).AppendEditDeleteDates(DateTime.Today);
            TrollCounterFileRepository.GetInstance().Save();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Troll Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
        if (System.Windows.MessageBox.Show("Are you sure you want to delete selected examination", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            if (selectedExamination.appointment.AddDays(-2) < DateTime.Now)
            {
                ScheduleEditRequestFileRepository.GetInstance().AddDeleteRequest(selectedExamination);
            }
            else
            {
                dataGrid.Items.Remove(selectedExamination);
                examinationRepository.DeleteExamination(selectedExamination.id);
                selectedExamination.doctor.examinations.Remove(selectedExamination);
            }
        }
    }

    private void dataGrid_Loaded(object sender, RoutedEventArgs e)
    {
        LoadGrid();
    }

    private void LoadGrid()
    {
        foreach (Examination examination in ExaminationRepository.GetInstance().examinations)
        {
            if (examination.medicalRecord.patient.username.Equals(loggedPatient.username))

                dataGrid.Items.Add(examination);
        }
        dataGrid.Items.Refresh();
    }
}