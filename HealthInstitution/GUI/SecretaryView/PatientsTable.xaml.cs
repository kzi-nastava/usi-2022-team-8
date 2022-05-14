using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Repository;
using HealthInstitution.GUI.SecretaryView;
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

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for PatientsTable.xaml
    /// </summary>
    public partial class PatientsTable : Window
    {
        public PatientsTable()
        {
            InitializeComponent();
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Patient> patients = PatientRepository.GetInstance().Patients;
            foreach (Patient patient in patients)
            {
                dataGrid.Items.Add(patient);
            }
            dataGrid.Items.Refresh();
        }
        private void CreatePatient_Click(object sender, RoutedEventArgs e)
        {
            CreatePatientDialog createPatientDialog = new CreatePatientDialog();
            createPatientDialog.ShowDialog();
            LoadRows();
        }

        private void UpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            Patient selectedPatient = (Patient)dataGrid.SelectedItem;
            if (selectedPatient != null) 
            {
                UpdatePatientWindow updatePatientWindow = new UpdatePatientWindow(selectedPatient);
                updatePatientWindow.ShowDialog();
                dataGrid.SelectedItem = null;
                LoadRows();
                
            }
        }

        private void DeletePatient_Click(object sender, RoutedEventArgs e)
        {
            Patient selectedPatient = (Patient)dataGrid.SelectedItem;
            if (selectedPatient != null)
            {
                ExaminationRepository examinationRepository = ExaminationRepository.GetInstance();
                OperationRepository operationRepository = OperationRepository.GetInstance();
                if (examinationRepository.GetPatientExaminations(selectedPatient).Count() == 0 && operationRepository.GetPatientOperations(selectedPatient).Count() == 0)
                {
                    PatientRepository.GetInstance().Delete(selectedPatient.Username);
                    dataGrid.SelectedItem = null;
                    LoadRows();
                }
                else
                {
                    System.Windows.MessageBox.Show("The patient must not be deleted.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void BlockPatient_Click(object sender, RoutedEventArgs e)
        {
            Patient selectedPatient = (Patient)dataGrid.SelectedItem;
            if (selectedPatient != null)
            {
                PatientRepository patientRepository = PatientRepository.GetInstance();
                patientRepository.ChangeBlockedStatus(selectedPatient.Username);
                dataGrid.SelectedItem = null;
                LoadRows();
            }
        }
    }
}
