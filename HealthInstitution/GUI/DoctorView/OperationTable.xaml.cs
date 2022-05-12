using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Repository;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for OperationTable.xaml
    /// </summary>
    public partial class OperationTable : Window
    {
        private OperationRepository _operationRepository = OperationRepository.GetInstance();
        private DoctorRepository _doctorRepository = DoctorRepository.GetInstance();
        private OperationDoctorRepository _operationDoctorRepository = OperationDoctorRepository.GetInstance();  
        private Doctor _loggedDoctor;
        public OperationTable(Doctor doctor)
        {
            this._loggedDoctor = doctor;
            InitializeComponent();
            LoadRows();
        }
        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Operation> doctorOperations = this._loggedDoctor.Operations;
            foreach (Operation operation in doctorOperations)
            {
                dataGrid.Items.Add(operation);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new AddOperationDialog(this._loggedDoctor).ShowDialog();
            LoadRows();
            dataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Operation selectedOperation = (Operation)dataGrid.SelectedItem;
            new EditOperationDialog(selectedOperation).ShowDialog();
            LoadRows();
            dataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var answer = System.Windows.MessageBox.Show("Are you sure you want to delete selected examination", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (answer == MessageBoxResult.Yes)
            {
                Operation selectedOperation = (Operation)dataGrid.SelectedItem;
                dataGrid.Items.Remove(selectedOperation);
                _operationRepository.Delete(selectedOperation.Id);
                _doctorRepository.DeleteOperation(_loggedDoctor, selectedOperation);
                _operationDoctorRepository.Save();
            }
        }
    }
}
