using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Ingredients;
using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.Ingredients.Repository;
using HealthInstitution.GUI.ManagerView.IngredientView;
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

namespace HealthInstitution.GUI.ManagerView
{
    /// <summary>
    /// Interaction logic for IngredientsTableWindow.xaml
    /// </summary>
    public partial class IngredientsTableWindow : Window
    {
        IIngredientService _ingredientService;
        public IngredientsTableWindow(IIngredientService ingredientService)
        {
            InitializeComponent();
            _ingredientService = ingredientService;
            LoadRows();
        }

        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void LoadRows()
        {
            dataGrid.Items.Clear();
            List<Ingredient> ingredients = _ingredientService.GetAll();
            foreach (Ingredient ingredient in ingredients)
            {
                dataGrid.Items.Add(ingredient);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddIngredientDialog addIngredientDialog = new AddIngredientDialog();
            addIngredientDialog.ShowDialog();

            LoadRows();
            dataGrid.Items.Refresh();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Ingredient selectedIngredient = (Ingredient)dataGrid.SelectedItem;

            EditIngredientDialog editIngredientDialog = new EditIngredientDialog(selectedIngredient);
            editIngredientDialog.ShowDialog();
            dataGrid.SelectedItem = null;
            dataGrid.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Ingredient selectedIngredient = (Ingredient)dataGrid.SelectedItem;
            
            if (_ingredientService.CheckOccurrenceOfIngredient(selectedIngredient))
            {
                System.Windows.MessageBox.Show("You cant delete ingredient because it's part of the drug!", "Delete error", MessageBoxButton.OK, MessageBoxImage.Error);
                dataGrid.SelectedItem = null;
                return;
            }

            if (System.Windows.MessageBox.Show("Are you sure you want to delete selected ingredient", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                dataGrid.Items.Remove(selectedIngredient);
                _ingredientService.Delete(selectedIngredient.Id);

            }
        }

        
    }
}
