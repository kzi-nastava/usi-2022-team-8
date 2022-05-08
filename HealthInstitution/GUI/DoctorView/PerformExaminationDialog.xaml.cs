using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for PerformExaminationDialog.xaml
    /// </summary>
    public partial class PerformExaminationDialog : Window
    {
        private Examination _selectedExamination;
        private MedicalRecord _medicalRecord;
        public PerformExaminationDialog(Examination examination)
        {
            InitializeComponent();
            this._selectedExamination= examination;
            _medicalRecord = this._selectedExamination.MedicalRecord;
            patientTextBox.Text = _medicalRecord.Patient.ToString();
            heightTextBox.Text = _medicalRecord.Height.ToString();
            weightTextBox.Text = _medicalRecord.Weight.ToString();
            foreach (String illness in _medicalRecord.PreviousIllnesses)
                illnessListBox.Items.Add(illness);
            foreach (String allergen in _medicalRecord.Allergens)
                allergenListBox.Items.Add(allergen);
        }

        private void AddIllness_Click(object sender, RoutedEventArgs e)
        {
            if (illnessesTextBox.Text.Trim() != "")
            {
                illnessListBox.Items.Add(illnessesTextBox.Text);
                illnessListBox.Items.Refresh();
            }
        }

        private void AddAllergen_Click(object sender, RoutedEventArgs e)
        {
            if (illnessesTextBox.Text.Trim() != "")
            {
                allergenListBox.Items.Add(allergensTextBox.Text);
                allergenListBox.Items.Refresh();
            }
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
                List<Prescription> prescriptions = _medicalRecord.Prescriptions;
                List<Referral> referrals = _medicalRecord.Referrals;
                MedicalRecordRepository.GetInstance().Update(_medicalRecord.Patient, height, weight, previousIllnesses, allergens, prescriptions, referrals);
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

        private void CreateReferral_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = _medicalRecord.Patient;
            Doctor doctor = _selectedExamination.Doctor;
            AddPrescriptionDialog dialog = new AddPrescriptionDialog(doctor, patient);
            dialog.ShowDialog();
        }
    }
}
