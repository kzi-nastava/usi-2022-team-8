﻿using HealthInstitution.Core.DIContainer;
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
    /// Interaction logic for RejectedDrugsTableWindow.xaml
    /// </summary>
    public partial class RejectedDrugsTableWindow : Window
    {
        IDrugService _drugService;
        IDrugVerificationService _drugVerificationService;
        public RejectedDrugsTableWindow(IDrugService drugService, IDrugVerificationService drugVerificationService)
        {
            InitializeComponent();
            _drugService = drugService;
            _drugVerificationService = drugVerificationService;
            LoadRows();
            reasonButton.IsEnabled = false;
            reviseButton.IsEnabled = false;
            deleteButton.IsEnabled = false;
        }

        private void LoadRows()
        {
            drugsDataGrid.Items.Clear();
            List<Drug> drugs = _drugService.GetAllRejected();
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
                List<Ingredient> ingredients = _drugService.GetIngredients(selectedDrug);
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

            reasonLabel.Content = "Reject reason: " + _drugVerificationService.ReasonForRejection(selectedDrug);
        }

        private void ReviseButton_Click(object sender, RoutedEventArgs e)
        {
            reasonLabel.Content = "";
            Drug selectedDrug = (Drug)drugsDataGrid.SelectedItem;

            ReviseDrugDialog reviseDrugDialog = DIContainer.GetService<ReviseDrugDialog>();
            reviseDrugDialog.SetDrug(selectedDrug);           
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
                _drugService.Delete(selectedDrug.Id);

            }
        }
    }
}
