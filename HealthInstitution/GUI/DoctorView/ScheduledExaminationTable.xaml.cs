using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
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

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for ScheduledExaminationTable.xaml
    /// </summary>
    public partial class ScheduledExaminationTable : Window
    {
        private Doctor _loggedDoctor;
        ITimetableService _timetableService;
        IExaminationService _examinationService;
        public ScheduledExaminationTable(ITimetableService timetableService, IExaminationService examinationService)
        {
            this._timetableService = timetableService;
            this._examinationService = examinationService;
            InitializeComponent();
            examinationRadioButton.IsChecked = true;
            datePicker.SelectedDate = DateTime.Now;
        }
        public void SetLoggedDoctor(Doctor doctor)
        {
            _loggedDoctor = doctor;
        }

        private void LoadOperationRows()
        {
            dataGrid.Items.Clear();
            List<Operation> scheduledOperations = _timetableService.GetScheduledOperations(_loggedDoctor);
            List<Operation> selectedOperations;
            if (upcomingDaysRadioButton.IsChecked == true)
            {
                selectedOperations = _timetableService.GetOperationsInThreeDays(scheduledOperations);
            }
            else
            {
                DateTime date = datePicker.SelectedDate.Value.Date;
                selectedOperations = _timetableService.GetOperationsByDate(scheduledOperations, date);
            }
            foreach (Operation operation in selectedOperations)
            {
                dataGrid.Items.Add(operation);
            }
        }

        public void LoadExaminationRows()
        {
            dataGrid.Items.Clear();
            List<Examination> scheduledExaminations = _timetableService.GetScheduledExaminations(_loggedDoctor);
            List<Examination> selectedExaminations;
            if ((bool)upcomingDaysRadioButton.IsChecked)
            {
                selectedExaminations = _timetableService.GetExaminationsInThreeDays(scheduledExaminations);
            } else
            {
                DateTime date = datePicker.SelectedDate.Value.Date;
                selectedExaminations = _timetableService.GetExaminationsByDate(scheduledExaminations, date);
            }
            foreach (var examination in selectedExaminations)
            {
                dataGrid.Items.Add(examination);
            }
    
        }
        private void Show_Click(object sender, RoutedEventArgs e)
        {
            if (examinationRadioButton.IsChecked == true)
            {
                LoadExaminationRows();
            } else
            {
                LoadOperationRows();
            }
        }

        private void ShowMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecord selectedMedicalRecord;
            if ((bool)examinationRadioButton.IsChecked)
            {
                Examination selectedExamination = (Examination)dataGrid.SelectedItem;
                selectedMedicalRecord = selectedExamination.MedicalRecord;
            }
            else
            {
                Operation selectedOperation = (Operation)dataGrid.SelectedItem;
                selectedMedicalRecord = selectedOperation.MedicalRecord;
            }
            new MedicalRecordDialog(selectedMedicalRecord).ShowDialog();
        }

        private bool IsExaminationSelected()
        {
            if (!(bool)examinationRadioButton.IsChecked)
            {
                System.Windows.MessageBox.Show("You have to check examination for it to start!", "Alert", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return false;
            }
            else if (dataGrid.SelectedIndex == -1)
            {
                System.Windows.MessageBox.Show("You have to select row to start examination!", "Alert", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private void StartExamination_Click(object sender, RoutedEventArgs e)
        {
            if (IsExaminationSelected())
            { 
                Examination selectedExamination = (Examination)dataGrid.SelectedItem;
                if (_examinationService.IsReadyForPerforming(selectedExamination))
                    new PerformExaminationDialog(selectedExamination, DIContainer.GetService<IExaminationService>(), DIContainer.GetService<IMedicalRecordService>()).ShowDialog();
                else
                    System.Windows.MessageBox.Show("Date of examination didn't pass!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            dataGrid.Items.Refresh();
        }

        private void AppointmentChecked(object sender, RoutedEventArgs e)
        {
        }

        private void DatesChecked(object sender, RoutedEventArgs e)
        {
        }
    }
}
