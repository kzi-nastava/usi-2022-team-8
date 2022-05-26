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
    /// Interaction logic for AddIngredientDialog.xaml
    /// </summary>
    public partial class AddIngredientDialog : Window
    {
        private IngredientRepository _ingredientRepository = IngredientRepository.GetInstance();
        public AddIngredientDialog()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string name = nameBox.Text;
            if (!ValidateIngredientName(name))
            {
                return;
            }

            _ingredientRepository.Add(name);
            System.Windows.MessageBox.Show("Ingredient added!", "Ingredient creation", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private bool ValidateIngredientName(string name)
        {           
            if (name.Trim() == "")
            {
                System.Windows.MessageBox.Show("Must input ingredient name!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (_ingredientRepository.Contains(name))
            {
                System.Windows.MessageBox.Show("This ingredient name already exist!", "Create error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
