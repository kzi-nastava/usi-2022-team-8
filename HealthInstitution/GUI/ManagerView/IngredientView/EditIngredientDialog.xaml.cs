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

namespace HealthInstitution.GUI.ManagerView.IngredientView
{
    /// <summary>
    /// Interaction logic for EditIngredientDialog.xaml
    /// </summary>
    public partial class EditIngredientDialog : Window
    {
        private Ingredient _ingredient;
        IIngredientService _ingredientService;
        public EditIngredientDialog(IIngredientService ingredientService)
        {
            InitializeComponent();
            _ingredientService = ingredientService;
            SetRoomData();
        }
        public void SetSelectedIngredient(Ingredient ingredient)
        {
            _ingredient = ingredient;
        }
        private void SetRoomData()
        {
            nameBox.Text = _ingredient.Name;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text;
            if (!ValidateIngredientName(name))
            {
                return;
            }

            _ingredientService.Update(_ingredient.Id, name);
            System.Windows.MessageBox.Show("Ingredient edited!", "Ingredient edit", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private bool ValidateIngredientName(string name)
        {
            if (name.Trim() == "")
            {
                System.Windows.MessageBox.Show("Must input ingredient name!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (_ingredientService.Contains(name))
            {
                System.Windows.MessageBox.Show("This ingredient name already exist!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
