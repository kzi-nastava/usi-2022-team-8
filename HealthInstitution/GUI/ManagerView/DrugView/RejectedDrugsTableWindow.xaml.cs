using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Ingredients.Model;
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

namespace HealthInstitution.GUI.ManagerView.DrugView
{
    /// <summary>
    /// Interaction logic for RejectedDrugsTableWindow.xaml
    /// </summary>
    public partial class RejectedDrugsTableWindow : Window
    {
        private DrugRepository _drugRepository = DrugRepository.GetInstance();
        public RejectedDrugsTableWindow()
        {
            InitializeComponent();
            LoadRows();
            reasonButton.IsEnabled = false;
            reviseButton.IsEnabled = false;
            deleteButton.IsEnabled = false;
        }

        private void LoadRows()
        {
            drugsDataGrid.Items.Clear();
            List<Drug> drugs = DrugService.GetAllRejected();
            foreach (Drug drug in drugs)
            {
                drugsDataGrid.Items.Add(drug);
            }
            ingredientsDataGrid.Items.Clear();
        }

        private void DrugsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (drugsDataGrid.SelectedItems.Count > 0)
            {
                reasonButton.IsEnabled = true;
                reviseButton.IsEnabled = true;
                deleteButton.IsEnabled = true;

                reasonLabel.Content = "";

                ingredientsDataGrid.Items.Clear();
                Drug selectedDrug = (Drug)drugsDataGrid.SelectedItem;
                List<Ingredient> ingredients = selectedDrug.Ingredients;
                foreach (Ingredient ingredient in ingredients)
                {
                    ingredientsDataGrid.Items.Add(ingredient);
                }
            }
            else
            {
                ingredientsDataGrid.Items.Clear();

                reasonButton.IsEnabled = false;
                reviseButton.IsEnabled = false;
                deleteButton.IsEnabled = false;

                reasonLabel.Content = "";
            }
        }

        private void ReasonButton_Click(object sender, RoutedEventArgs e)
        {
            Drug selectedDrug = (Drug)drugsDataGrid.SelectedItem;

            reasonLabel.Content = "Reject reason: " + selectedDrug.RejectionReason;
        }

        private void ReviseButton_Click(object sender, RoutedEventArgs e)
        {
            reasonLabel.Content = "";
            Drug selectedDrug = (Drug)drugsDataGrid.SelectedItem;

            ReviseDrugDialog reviseDrugDialog = new ReviseDrugDialog(selectedDrug);
            reviseDrugDialog.ShowDialog();
            drugsDataGrid.SelectedItem = null;
            LoadRows();
            drugsDataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            reasonLabel.Content = "";
            Drug selectedDrug = (Drug)drugsDataGrid.SelectedItem;

            if (System.Windows.MessageBox.Show("Are you sure you want to delete selected drug", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                drugsDataGrid.Items.Remove(selectedDrug);
                DrugService.Delete(selectedDrug.Id);

            }
        }
    }
}
