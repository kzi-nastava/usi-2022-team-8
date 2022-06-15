using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users;
using HealthInstitution.GUI.SecretaryView;
using System.Windows;

namespace HealthInstitution.GUI.UserWindow
{
    /// <summary>
    /// Interaction logic for PatientsTable.xaml
    /// </summary>
    public partial class PatientsTable : Window
    {
        IPatientService _patientService;
        public PatientsTable(IPatientService patientService)
        {
            InitializeComponent();
            _patientService = patientService;
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Patient> patients = _patientService.GetAll();
            foreach (Patient patient in patients)
            {
                dataGrid.Items.Add(patient);
            }
            dataGrid.Items.Refresh();
        }
        private void CreatePatient_Click(object sender, RoutedEventArgs e)
        {
            CreatePatientDialog createPatientDialog = DIContainer.GetService<CreatePatientDialog>();          
            createPatientDialog.ShowDialog();
            LoadRows();
        }

        private void UpdatePatient_Click(object sender, RoutedEventArgs e)
        {
            Patient selectedPatient = (Patient)dataGrid.SelectedItem;
            if (selectedPatient != null) 
            {
                UpdatePatientWindow updatePatientWindow = DIContainer.GetService<UpdatePatientWindow>();
                updatePatientWindow.SetSelectedPatient(selectedPatient);               
                updatePatientWindow.ShowDialog();

                dataGrid.SelectedItem = null;
                LoadRows();
            }
        }
        private void TryDeletingPatient(Patient selectedPatient)
        {
            if (_patientService.IsAvailableForDeletion(selectedPatient))
            {
                _patientService.Delete(selectedPatient.Username);
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
                _patientService.ChangeBlockedStatus(selectedPatient.Username);
                dataGrid.SelectedItem = null;
                LoadRows();
            }
        }
    }
}
