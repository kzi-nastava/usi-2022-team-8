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
        public Examination selectedExamination { get; set; }
        public PerformExaminationDialog(Examination selectedExamination)
        {
            InitializeComponent();
            this.selectedExamination= selectedExamination;
            MedicalRecord medicalRecord = this.selectedExamination.medicalRecord;
            patientTextBox.Text = medicalRecord.patient.ToString();
            heightTextBox.Text = medicalRecord.height.ToString();
            weightTextBox.Text = medicalRecord.weight.ToString();
            foreach (String illness in medicalRecord.previousIllnesses)
                illnessListBox.Items.Add(illness);
            foreach (String allergen in medicalRecord.allergens)
                allergenListBox.Items.Add(allergen);
        }

        private void AddIllness_Click(object sender, RoutedEventArgs e)
        {
            illnessListBox.Items.Add(illnessesTextBox.Text);
            illnessListBox.Items.Refresh();
        }

        private void AddAllergen_Click(object sender, RoutedEventArgs e)
        {
            allergenListBox.Items.Add(allergensTextBox.Text);
            allergenListBox.Items.Refresh();
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
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
                MedicalRecord medicalRecord = this.selectedExamination.medicalRecord;
                List<Prescription> prescriptions = medicalRecord.prescriptions;
                List<Referral> referrals = medicalRecord.referrals;
                MedicalRecordRepository.GetInstance().UpdateMedicalRecord(medicalRecord.patient, height, weight, previousIllnesses, allergens, prescriptions, referrals);
                this.selectedExamination.anamnesis = anamnesisTextBox.Text;
                this.selectedExamination.status = ExaminationStatus.Completed;
                ExaminationRepository.GetInstance().SaveExaminations();
                System.Windows.MessageBox.Show("You have finished the examination!", "Congrats", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                this.Close();
            }
            catch
            {
                System.Windows.MessageBox.Show("You haven't fulfilled it the right way!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);

            }
        }
    }
}
