using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.GUI.SecretaryView;
using System.Windows;

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for PatientsTable.xaml
    /// </summary>
    public partial class PatientsTable : Window
    {
        ExaminationRepository _examinationRepository = ExaminationRepository.GetInstance();
        OperationRepository _operationRepository = OperationRepository.GetInstance();
        PatientRepository _patientRepository = PatientRepository.GetInstance();
        public PatientsTable()
        {
            InitializeComponent();
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Patient> patients = _patientRepository.Patients;
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
        private void TryDeletingPatient(Patient selectedPatient)
        {
            if (_examinationRepository.GetPatientExaminations(selectedPatient).Count() == 0 && OperationService.GetPatientOperations(selectedPatient).Count() == 0)
            {
                _patientRepository.Delete(selectedPatient.Username);
                dataGrid.SelectedItem = null;
                LoadRows();
            }
            else
            {
                System.Windows.MessageBox.Show("The patient must not be deleted.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void DeletePatient_Click(object sender, RoutedEventArgs e)
        {
            Patient selectedPatient = (Patient)dataGrid.SelectedItem;
            if (selectedPatient != null)
            {
                TryDeletingPatient(selectedPatient);
            }
        }
        private void BlockPatient_Click(object sender, RoutedEventArgs e)
        {
            Patient selectedPatient = (Patient)dataGrid.SelectedItem;
            if (selectedPatient != null)
            {
                PatientRepository patientRepository = PatientRepository.GetInstance();
                PatientService.ChangeBlockedStatus(selectedPatient.Username);
                dataGrid.SelectedItem = null;
                LoadRows();
            }
        }
    }
}
