using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
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
            this._selectedExamination = examination;
            Load();
        }

        private void Load()
        {
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
            String illness = illnessTextBox.Text.Trim();
            if (illness != "")
            {
                illnessListBox.Items.Add(illness);
                illnessListBox.Items.Refresh();
                illnessTextBox.Clear();
            }
        }

        private void AddAllergen_Click(object sender, RoutedEventArgs e)
        {
            string allergen = allergenTextBox.Text.Trim();
            if (allergen != "")
            {
                allergenListBox.Items.Add(allergen);
                allergenListBox.Items.Refresh();
                allergenTextBox.Clear();
            }
        }
        private void CreateReferral_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = _medicalRecord.Patient;
            Doctor doctor = _selectedExamination.Doctor;
            AddReferralDialog dialog = new AddReferralDialog(doctor, patient);
            dialog.ShowDialog();
        }

        private void CreatePrescription_Click(object sender, RoutedEventArgs e)
        {
            AddPrescriptionDialog dialog = new AddPrescriptionDialog(_medicalRecord);
            dialog.ShowDialog();
        }

        private MedicalRecordDTO CreateMedicalRecordDTOFromInputData()
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
            return new MedicalRecordDTO(height, weight, previousIllnesses, allergens, _selectedExamination.MedicalRecord.Patient, prescriptions, referrals);
        }

        private void UpdateExaminationFromInputData()
        {
            this._selectedExamination.Anamnesis = anamnesisTextBox.Text;
            this._selectedExamination.Status = ExaminationStatus.Completed;
            ExaminationRepository.GetInstance().Save();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MedicalRecordDTO medicalRecordDTO = CreateMedicalRecordDTOFromInputData();
                MedicalRecordService.Update(medicalRecordDTO);
                UpdateExaminationFromInputData();
                System.Windows.MessageBox.Show("You have finished the examination!", "Congrats", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                new ConsumedEquipmentDialog(_selectedExamination.Room).ShowDialog();

            }
            catch
            {
                System.Windows.MessageBox.Show("You haven't fulfilled it the right way!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
        }
    }
}
