using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Ingredients;
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
    /// Interaction logic for DrugsOnVerificationTableWindow.xaml
    /// </summary>
    public partial class DrugsOnVerificationTableWindow : Window
    {
        IDrugService _drugService;
        public DrugsOnVerificationTableWindow(IDrugService drugService)
        {
            InitializeComponent();
            _drugService = drugService;
            LoadRows();
            editButton.IsEnabled = false;
            deleteButton.IsEnabled = false;
        }

        private void LoadRows()
        {
            drugsDataGrid.Items.Clear();
            List<Drug> drugs = _drugService.GetAllCreated();
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
                editButton.IsEnabled = true;
                deleteButton.IsEnabled = true;

                ingredientsDataGrid.Items.Clear();
                Drug selectedDrug = (Drug)drugsDataGrid.SelectedItem;
                List<Ingredient> ingredients = _drugService.GetIngredients(selectedDrug);
                foreach (Ingredient ingredient in ingredients)
                {
                    ingredientsDataGrid.Items.Add(ingredient);
                }
            } else
            {
                ingredientsDataGrid.Items.Clear();

                editButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddDrugDialog addDrugDialog = DIContainer.GetService<AddDrugDialog>();           
            addDrugDialog.ShowDialog();

            LoadRows();
            drugsDataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Drug selectedDrug = (Drug)drugsDataGrid.SelectedItem;

            EditDrugDialog editDrugDialog = DIContainer.GetService<EditDrugDialog>();
            editDrugDialog.SetDrug(selectedDrug);            
            editDrugDialog.ShowDialog();

            drugsDataGrid.SelectedItem = null;
            drugsDataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Drug selectedDrug = (Drug)drugsDataGrid.SelectedItem;

            if (System.Windows.MessageBox.Show("Are you sure you want to delete selected drug", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                drugsDataGrid.Items.Remove(selectedDrug);
                _drugService.Delete(selectedDrug.Id);

            }
        }

    }
}
