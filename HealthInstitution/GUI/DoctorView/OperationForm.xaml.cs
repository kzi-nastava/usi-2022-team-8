using HealthInstitution.Core.Operations.Model;
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
    /// Interaction logic for OperationForm.xaml
    /// </summary>
    public partial class OperationForm : Window
    {
        public OperationForm()
        {
            InitializeComponent();
        }

        private void addButton_click(object sender, RoutedEventArgs e)
        {
            AddOpeationDialog addOpeationDialog = new AddOpeationDialog();
            addOpeationDialog.ShowDialog();
        }

        private void deleteButton_click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.MessageBox.Show("Are you sure you want to delete selected examination", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Operation selectedExamination = (Operation)dataGrid.SelectedItem;
            }
        }

        private void editButton_click(object sender, RoutedEventArgs e)
        {
            Operation selectedOperation = (Operation)dataGrid.SelectedItem;
            EditOperationDialog editOperationDialog = new EditOperationDialog(selectedOperation);
            editOperationDialog.ShowDialog();
        }
    }
}
