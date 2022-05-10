﻿using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Prescriptions.Repository;
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
    /// Interaction logic for AddPrescriptionDialog.xaml
    /// </summary>
    public partial class AddPrescriptionDialog : Window
    {
        private MedicalRecord _medicalRecord;
        private DrugRepository _drugRepository = DrugRepository.GetInstance();
        private PrescriptionRepository _prescriptionRepository = PrescriptionRepository.GetInstance();
        private MedicalRecordRepository _medicalRecordRepository = MedicalRecordRepository.GetInstance();
        public AddPrescriptionDialog(MedicalRecord medicalRecord)
        {
            InitializeComponent();
            this._medicalRecord = medicalRecord;
        }

        private void TimeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var timeComboBox = sender as System.Windows.Controls.ComboBox;
            timeComboBox.Items.Add("Not important");
            timeComboBox.Items.Add("Pre meal");
            timeComboBox.Items.Add("During meal");
            timeComboBox.Items.Add("Post meal");
            timeComboBox.SelectedIndex = 0;
            timeComboBox.Items.Refresh();
        }

        private void DrugComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var drugComboBox = sender as System.Windows.Controls.ComboBox;
            List<Drug> drugs = _drugRepository.GetAll();
            foreach (Drug drug in drugs)
            {
                drugComboBox.Items.Add(drug);
            }
            /*drugComboBox.ItemsSource = drugs;*/
            drugComboBox.SelectedIndex = 0;
            drugComboBox.Items.Refresh();
        }

        private void CollectForms()
        {
            Drug drug = (Drug)drugComboBox.SelectedItem;
            PrescriptionTime timeOfUse = (PrescriptionTime)timeComboBox.SelectedIndex;
            int dailyDose = Int32.Parse(doseTextBox.Text);
        }

        private bool IsPatientAlergic(List<Ingredient> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                if (_medicalRecord.Allergens.Contains(ingredient.Name))
                {
                    System.Windows.MessageBox.Show("Patient is alergic to the ingredients of this drug!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }
            return false;
        }

        private PrescriptionDTO CreatePrescriptionByForms()
        {
            Drug drug = (Drug)drugComboBox.SelectedItem;
            PrescriptionTime timeOfUse = (PrescriptionTime)timeComboBox.SelectedIndex;
            int dailyDose = Int32.Parse(doseTextBox.Text);
            PrescriptionDTO prescription = new PrescriptionDTO(dailyDose, timeOfUse, drug);
            return prescription;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrescriptionDTO prescriptionDTO = CreatePrescriptionByForms();
                if (!IsPatientAlergic(prescriptionDTO.Drug.Ingredients))
                {
                    Prescription prescription = _prescriptionRepository.Add(prescriptionDTO);
                    _medicalRecordRepository.AddPrescription(_medicalRecord.Patient, prescription);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}