using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Ingredients;
using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.Ingredients.Repository;
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
    /// Interaction logic for ReviseDrugDialog.xaml
    /// </summary>
    public partial class ReviseDrugDialog : Window
    {
        private DrugRepository _drugRepository = DrugRepository.GetInstance();
        private List<Ingredient> _ingredientsForDrug;
        private Drug _drug;
        public ReviseDrugDialog(Drug drug)
        {
            InitializeComponent();
            _drug = drug;
            _ingredientsForDrug = new List<Ingredient>();
            addIngredient.IsEnabled = false;
            SetDrugData();
        }

        private void SetDrugData()
        {
            nameBox.Text = _drug.Name;

            foreach (Ingredient ingredient in _drug.Ingredients)
            {
                _ingredientsForDrug.Add(ingredient);
            }
            RefreshDataGrid();
        }
        private void IngredientsComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<Ingredient> ingredients = IngredientService.GetAll();
            ingredientsComboBox.ItemsSource = ingredients;
            ingredientsComboBox.SelectedItem = null;
        }

        private void IngredientsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ingredientsComboBox.SelectedItem != null)
            {
                addIngredient.IsEnabled = true;
            }
            else
            {
                addIngredient.IsEnabled = false;
            }
        }

        private void RefreshDataGrid()
        {
            dataGrid.Items.Clear();
            List<Ingredient> ingredients = _ingredientsForDrug;
            foreach (Ingredient ingredient in ingredients)
            {
                dataGrid.Items.Add(ingredient);
            }
            dataGrid.Items.Refresh();
        }

        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ingredient ingredient = (Ingredient)ingredientsComboBox.SelectedItem;
            if (_ingredientsForDrug.Contains(ingredient))
            {
                System.Windows.MessageBox.Show("Ingredient is already part of drug!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _ingredientsForDrug.Add(ingredient);
            RefreshDataGrid();
            ingredientsComboBox.SelectedItem = null;
        }

        private void RemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ingredient ingredient = (Ingredient)dataGrid.SelectedItem;
            _ingredientsForDrug.Remove(ingredient);
            RefreshDataGrid();
        }

        private void Revise_Click(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text;
            if (!ValidateDrugName(name))
            {
                return;
            }

            DrugDTO drugDTO = new DrugDTO(name, DrugState.Created, _ingredientsForDrug);
            _drugRepository.Update(_drug.Id, drugDTO);
            System.Windows.MessageBox.Show("Drug revised and waiting on verification!", "Ingredient creation", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private bool ValidateDrugName(string name)
        {
            if (name.Trim() == "")
            {
                System.Windows.MessageBox.Show("Must input drug name!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (_drug.Name != name && _drugRepository.Contains(name))
            {
                System.Windows.MessageBox.Show("This drug name already exist!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

    }
}
