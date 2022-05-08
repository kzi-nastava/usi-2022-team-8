using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Referrals.Model;
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
    /// Interaction logic for PerformExaminationDialog.xaml
    /// </summary>
    public partial class PerformExaminationDialog : Window
    {
        private Examination _selectedExamination;
        public PerformExaminationDialog(Examination examination)
        {
            InitializeComponent();
            this._selectedExamination= examination;
            MedicalRecord medicalRecord = this._selectedExamination.MedicalRecord;
            patientTextBox.Text = medicalRecord.Patient.ToString();
            heightTextBox.Text = medicalRecord.Height.ToString();
            weightTextBox.Text = medicalRecord.Weight.ToString();
            foreach (String illness in medicalRecord.PreviousIllnesses)
                illnessListBox.Items.Add(illness);
            foreach (String allergen in medicalRecord.Allergens)
                allergenListBox.Items.Add(allergen);
        }

        private void addIllness_Click(object sender, RoutedEventArgs e)
        {
            if (illnessesTextBox.Text.Trim() != "")
            {
                illnessListBox.Items.Add(illnessesTextBox.Text);
                illnessListBox.Items.Refresh();
            }
        }

        private void addAllergen_Click(object sender, RoutedEventArgs e)
        {
            if (illnessesTextBox.Text.Trim() != "")
            {
                allergenListBox.Items.Add(allergensTextBox.Text);
                allergenListBox.Items.Refresh();
            }
        }

        private void finish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double height = Double.Parse(heightTextBox.Text);
                double weight = Double.Parse(weightTextBox.Text);
                List<String> previousIllnesses = new List<String>();
                foreach (String illness in illnessListBox.Items)
                {
                    previousIllnesses.Add(illness);
                };
                List<String> allergens = new List<String>();
                foreach (String allergen in allergenListBox.Items)
                {
                    allergens.Add(allergen);
                }
                MedicalRecord medicalRecord = this._selectedExamination.MedicalRecord;
                List<Prescription> prescriptions = medicalRecord.Prescriptions;
                List<Referral> referrals = medicalRecord.Referrals;
                MedicalRecordRepository.GetInstance().Update(medicalRecord.Patient, height, weight, previousIllnesses, allergens, prescriptions, referrals);
                this._selectedExamination.Anamnesis = anamnesisTextBox.Text;
                this._selectedExamination.Status = ExaminationStatus.Completed;
                ExaminationRepository.GetInstance().Save();
                System.Windows.MessageBox.Show("You have finished the examination!", "Congrats", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch
            {
                System.Windows.MessageBox.Show("You haven't fulfilled it the right way!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
}
