using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
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
        Doctor loggedDoctor;
        public ScheduledExaminationTable(Doctor doctor)
        {
            this.loggedDoctor = doctor;   
            InitializeComponent();
            examinationRadioButton.IsChecked = true;
            datePicker.SelectedDate = DateTime.Now;
        }

        public void LoadOperationGridRows()
        {
            dataGrid.Items.Clear();
            List<Operation> doctorOperations = this.loggedDoctor.operations;
            if (upcomingDaysRadioButton.IsChecked == true)
            {
                DateTime today = DateTime.Now;
                DateTime dateForThreeDays = today.AddDays(3);
                foreach (Operation operation in doctorOperations)
                {
                    if (operation.appointment <= dateForThreeDays && operation.appointment >= today)
                        dataGrid.Items.Add(operation);
                }
            }
            else
            {
                foreach (Operation operation in doctorOperations)
                {
                    if (operation.appointment.Date == datePicker.SelectedDate.Value.Date)
                        dataGrid.Items.Add(operation);
                }
            }
        }

        public void LoadExaminationGridRows()
        {
            dataGrid.Items.Clear();
            List<Examination> doctorExaminations = this.loggedDoctor.examinations;
            if ((bool)upcomingDaysRadioButton.IsChecked)
            {
                DateTime today = DateTime.Now;
                DateTime dateForThreeDays = today.AddDays(3);
                foreach (Examination examination in doctorExaminations)
                {
                    if (examination.appointment <= dateForThreeDays && examination.appointment >= today)
                        dataGrid.Items.Add(examination);
                }
            } else
            {
                foreach (Examination examination in doctorExaminations)
                {
                    if (examination.appointment.Date == datePicker.SelectedDate.Value.Date)
                        dataGrid.Items.Add(examination);
                }
            }
    
        }
        private void Show_Click(object sender, RoutedEventArgs e)
        {
            if (examinationRadioButton.IsChecked == true)
            {
                LoadExaminationGridRows();
            } else
            {
                LoadOperationGridRows();
            }
        }

        private void ShowMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecord selectedMedicalRecord;
            if ((bool)examinationRadioButton.IsChecked)
            {
                Examination selectedExamination = (Examination)dataGrid.SelectedItem;
                selectedMedicalRecord = selectedExamination.medicalRecord;
            }
            else
            {
                Operation selectedOperation = (Operation)dataGrid.SelectedItem;
                selectedMedicalRecord = selectedOperation.medicalRecord;
            }
            new MedicalRecordDialog(selectedMedicalRecord).ShowDialog();
        }

        private void StartExamination_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)examinationRadioButton.IsChecked)
            {
                System.Windows.MessageBox.Show("You have to check examination for it to start!", "Alert", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            } else if (dataGrid.SelectedIndex == -1) {
                    System.Windows.MessageBox.Show("You have to select row to start examination!", "Alert", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            } else 
            { 
                Examination selectedExamination = (Examination)dataGrid.SelectedItem;
                if (selectedExamination.status == ExaminationStatus.Scheduled)
                {
                    new PerformExaminationDialog(selectedExamination).ShowDialog();
                } else
                {
                    System.Windows.MessageBox.Show("Examination is already completed!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                }
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
