using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
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
    /// Interaction logic for DrugsVerificationTable.xaml
    /// </summary>
    public partial class DrugsVerificationTable : Window
    {
        IDrugVerificationService _drugVerificationService;
        IDrugService _drugService;
        public DrugsVerificationTable(IDrugVerificationService drugVerificationService, IDrugService drugService)
        {
            _drugService = drugService;
            _drugVerificationService = drugVerificationService;
            InitializeComponent();
            LoadRows();
        }

        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Drug> drugs = _drugService.GetAllCreated();
            foreach (Drug drug in drugs)
            {
                dataGrid.Items.Add(drug);
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            Drug selectedDrug = (Drug)dataGrid.SelectedItem;
            System.Windows.MessageBox.Show("You have accepted a new drug!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            _drugVerificationService.Accept(selectedDrug);
            dataGrid.Items.Remove(selectedDrug);
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            Drug selectedDrug = (Drug)dataGrid.SelectedItem;
            DrugRejectionReasonDialog drugRejectionReasonDialog = new DrugRejectionReasonDialog(selectedDrug);
            drugRejectionReasonDialog.ShowDialog();
            LoadRows();
            dataGrid.Items.Refresh();
        }
    }
}
