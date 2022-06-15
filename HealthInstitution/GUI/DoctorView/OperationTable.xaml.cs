using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using HealthInstitution.Core.SystemUsers.Patients;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for OperationTable.xaml
    /// </summary>
    public partial class OperationTable : Window
    {
        private Doctor _loggedDoctor;
        IOperationService _operationService;
        IDoctorService _doctorService;
        public OperationTable(IOperationService operationService, IDoctorService doctorService)
        {
            this._operationService = operationService;
            _doctorService = doctorService;
            InitializeComponent();
            LoadRows();
        }
        public void SetLoggedDoctor(Doctor doctor)
        {
            _loggedDoctor = doctor;
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Operation> doctorOperations = _operationService.GetByDoctor(_loggedDoctor.Username);
            foreach (Operation operation in doctorOperations)
            {
                dataGrid.Items.Add(operation);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddOperationDialog dialog = DIContainer.GetService<AddOperationDialog>();
            dialog.SetLoggedDoctor(this._loggedDoctor);
            dialog.ShowDialog();
            
            LoadRows();
            dataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Operation selectedOperation = (Operation)dataGrid.SelectedItem;

            EditOperationDialog dialog = DIContainer.GetService<EditOperationDialog>();
            dialog.SetSelectedOperation(selectedOperation);
            dialog.ShowDialog();
      
            LoadRows();
            dataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var answer = System.Windows.MessageBox.Show("Are you sure you want to delete selected operation?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                Operation selectedOperation = (Operation)dataGrid.SelectedItem;
                dataGrid.Items.Remove(selectedOperation);
                _operationService.Delete(selectedOperation.Id);
                _doctorService.DeleteOperation(selectedOperation);
            }
        }
    }
}
