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
    /// Interaction logic for ScheduledExaminationForm.xaml
    /// </summary>
    public partial class ScheduledExaminationForm : Window
    {
        Doctor loggedDoctor;
        public ScheduledExaminationForm(Doctor loggedDoctor)
        {
            this.loggedDoctor = loggedDoctor;   
            InitializeComponent();
            ExaminationRadioButton.IsChecked = true;
        }

        public void LoadOperationsGridRows()
        {
            dataGrid.Items.Clear();
            List<Operation> doctorOperations = this.loggedDoctor.operations;
            if (UpcomingDaysRadioButton.IsChecked == true)
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
                DateTime today = DateTime.Now;
                foreach (Operation operation in doctorOperations)
                {
                    if (operation.appointment == today)
                        dataGrid.Items.Add(operation);
                }
            }
        }

        public void LoadExaminationsGridRows()
        {
            dataGrid.Items.Clear();
            List<Examination> doctorExaminations = this.loggedDoctor.examinations;
            if (UpcomingDaysRadioButton.IsChecked == true)
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
                DateTime today = DateTime.Now;
                foreach (Examination examination in doctorExaminations)
                {
                    if (examination.appointment == today)
                        dataGrid.Items.Add(examination);
                }
            }
    
        }
        private void Show_Click(object sender, RoutedEventArgs e)
        {
            if (ExaminationRadioButton.IsChecked == true)
            {
                LoadExaminationsGridRows();
            } else
            {
                LoadOperationsGridRows();
            }
        }

        /*[STAThread]
        static void Main(string[] args)
        {
            ScheduledExaminationForm window = new ScheduledExaminationForm();
            window.ShowDialog();
        }*/

        private void ShowMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            MedicalRecord selectedMedicalRecord;
            if (ExaminationRadioButton.IsChecked == true)
            {
                Examination selectedExamination = (Examination)dataGrid.SelectedItem;
                selectedMedicalRecord = selectedExamination.medicalRecord;
            }
            else
            {
                Operation selectedOperation = (Operation)dataGrid.SelectedItem;
                selectedMedicalRecord = selectedOperation.medicalRecord;
            }
            MedicalRecordDialog medicalRecordDialog = new MedicalRecordDialog(selectedMedicalRecord);
            medicalRecordDialog.ShowDialog();
        }

        private void StartExamination_Click(object sender, RoutedEventArgs e)
        {
            if (!(bool)ExaminationRadioButton.IsChecked)
            {
                System.Windows.MessageBox.Show("You have to check examination for it to start!", "Alert", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            } else
            {
                Examination selectedExamination = (Examination)dataGrid.SelectedItem;
                PerformExaminationDialog performExaminationDialog = new PerformExaminationDialog(selectedExamination);
                performExaminationDialog.ShowDialog();
            }
        }

        private void AppointmentChecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
