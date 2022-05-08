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
            loadRows();
        }
        private void loadRows()
        {
            dataGrid.Items.Clear();
            List<Operation> doctorOperations = this._loggedDoctor.Operations;
            foreach (Operation operation in doctorOperations)
            {
                dataGrid.Items.Add(operation);
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            new AddOperationDialog(this._loggedDoctor).ShowDialog();
            loadRows();
            dataGrid.Items.Refresh();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            Operation selectedOperation = (Operation)dataGrid.SelectedItem;
            new EditOperationDialog(selectedOperation).ShowDialog();
            loadRows();
            dataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to delete selected examination", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
