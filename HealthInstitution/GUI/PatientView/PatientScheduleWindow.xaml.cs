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
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.Core.TrollCounters.Repository;
using HealthInstitution.Core.ScheduleEditRequests.Repository;

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
            TrollCounterRepository.GetInstance().CheckTroll(loggedPatient.username);
            AddExaminationDialog addExaminationDialog = new AddExaminationDialog(loggedPatient);
            addExaminationDialog.ShowDialog();
            dataGrid.Items.Clear();
            LoadGrid();
            TrollCounterRepository.GetInstance().GetTrollCounterById(loggedPatient.username).AppendCreateDates(DateTime.Today);
            TrollCounterRepository.GetInstance().SaveTrollCounters();
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
            TrollCounterRepository.GetInstance().CheckTroll(loggedPatient.username);
            Examination selectedExamination = (Examination)dataGrid.SelectedItem;
            EditExaminationDialog editExaminationDialog = new EditExaminationDialog(selectedExamination);
            editExaminationDialog.ShowDialog();
            dataGrid.Items.Clear();
            LoadGrid();
            TrollCounterRepository.GetInstance().GetTrollCounterById(loggedPatient.username).AppendEditDeleteDates(DateTime.Today);
            TrollCounterRepository.GetInstance().SaveTrollCounters();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Troll Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
    }

    private void deleteButton_click(object sender, RoutedEventArgs e)
    {
        try
        {
            TrollCounterRepository.GetInstance().CheckTroll(loggedPatient.username);
            ExaminationDoctorRepository.GetInstance().SaveExaminationDoctor();
            dataGrid.Items.Clear();
            LoadGrid();
            TrollCounterRepository.GetInstance().GetTrollCounterById(loggedPatient.username).AppendEditDeleteDates(DateTime.Today);
            TrollCounterRepository.GetInstance().SaveTrollCounters();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Troll Alert", MessageBoxButton.OK, MessageBoxImage.Error);
            this.Close();
        }
        if (System.Windows.MessageBox.Show("Are you sure you want to delete selected examination", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            Examination selectedExamination = (Examination)dataGrid.SelectedItem;
            if (selectedExamination.appointment.AddDays(-2) < DateTime.Now)
            {
                ScheduleEditRequestRepository.GetInstance().DeleteScheduleEditRequests(selectedExamination.id);
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