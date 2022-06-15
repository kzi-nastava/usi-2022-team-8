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
        public OperationTable(Doctor doctor, IOperationService operationService, IDoctorService doctorService)
        {
            this._loggedDoctor = doctor;
            this._operationService = operationService;
            _doctorService = doctorService;
            InitializeComponent();
            LoadRows();
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
            new AddOperationDialog(this._loggedDoctor, DIContainer.GetService<IPatientService>(), DIContainer.GetService<IMedicalRecordService>(), DIContainer.GetService<ISchedulingService>()).ShowDialog();
            LoadRows();
            dataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Operation selectedOperation = (Operation)dataGrid.SelectedItem;
            new EditOperationDialog(selectedOperation, DIContainer.GetService<IPatientService>(), DIContainer.GetService<IMedicalRecordService>(), DIContainer.GetService<IOperationService>()).ShowDialog();
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
