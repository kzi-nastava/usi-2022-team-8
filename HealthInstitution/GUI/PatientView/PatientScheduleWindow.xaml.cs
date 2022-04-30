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
        AddExaminationDialog addExaminationDialog = new AddExaminationDialog(loggedPatient);
        addExaminationDialog.ShowDialog();
        dataGrid.Items.Clear();
        LoadGrid();
    }

    private void editButton_click(object sender, RoutedEventArgs e)
    {
        Examination selectedExamination = (Examination)dataGrid.SelectedItem;
        EditExaminationDialog editExaminationDialog = new EditExaminationDialog(selectedExamination);
        editExaminationDialog.ShowDialog();
        dataGrid.Items.Clear();
        LoadGrid();
    }

    private void deleteButton_click(object sender, RoutedEventArgs e)
    {
        if (System.Windows.MessageBox.Show("Are you sure you want to delete selected examination", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
        {
            Examination selectedExamination = (Examination)dataGrid.SelectedItem;
            dataGrid.Items.Remove(selectedExamination);
            examinationRepository.DeleteExamination(selectedExamination.id);
            selectedExamination.doctor.examinations.Remove(selectedExamination);
        }
        ExaminationDoctorRepository.GetInstance().SaveExaminationDoctor();
        dataGrid.Items.Clear();
        LoadGrid();
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