using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
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
        private DoctorRepository _doctorRepository = DoctorRepository.GetInstance();
        public ScheduledExaminationTable(Doctor doctor)
        {
            this._loggedDoctor = doctor;   
            InitializeComponent();
            examinationRadioButton.IsChecked = true;
            datePicker.SelectedDate = DateTime.Now;
        }

        private void LoadOperationRows()
        {
            dataGrid.Items.Clear();
            List<Operation> scheduledOperations = TimetableService.GetScheduledOperations(_loggedDoctor);
            List<Operation> selectedOperations = new List<Operation>();
            if (upcomingDaysRadioButton.IsChecked == true)
            {
                selectedOperations = TimetableService.GetOperationsInThreeDays(scheduledOperations);
            }
            else
            {
                DateTime date = datePicker.SelectedDate.Value.Date;
                selectedOperations = TimetableService.GetOperationsByDate(scheduledOperations, date);
            }
            foreach (Operation operation in selectedOperations)
            {
                dataGrid.Items.Add(operation);
            }
        }

        public void LoadExaminationRows()
        {
            dataGrid.Items.Clear();
            List<Examination> scheduledExaminations = TimetableService.GetScheduledExaminations(_loggedDoctor);
            List<Examination> selectedExaminations = new List<Examination>();
            if ((bool)upcomingDaysRadioButton.IsChecked)
            {
                selectedExaminations = TimetableService.GetExaminationsInThreeDays(scheduledExaminations);
            } else
            {
                DateTime date = datePicker.SelectedDate.Value.Date;
                selectedExaminations = TimetableService.GetExaminationsByDate(scheduledExaminations, date);
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

        private bool IsExaminationReadyForPerforming(Examination selectedExamination)
        {
            if (!(selectedExamination.Appointment <= DateTime.Now))
            {
                System.Windows.MessageBox.Show("Date of examination didn't pass!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        private void StartExamination_Click(object sender, RoutedEventArgs e)
        {
            bool isSelected = IsExaminationSelected();
            if (isSelected)
            { 
                Examination selectedExamination = (Examination)dataGrid.SelectedItem;
                if (IsExaminationReadyForPerforming(selectedExamination))
                    new PerformExaminationDialog(selectedExamination).ShowDialog();
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
