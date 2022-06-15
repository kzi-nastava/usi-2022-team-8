using HealthInstitution.Core.MedicalRecords.Model;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for MedicalRecordDialog.xaml
    /// </summary>
    public partial class MedicalRecordDialog : Window
    {
        private MedicalRecord _selectedMedicalRecord;
        public MedicalRecordDialog()
        {
            InitializeComponent();
            Load();
        }
        public void SetSelectedMedicalRecord(MedicalRecord medicalRecord)
        {
            _selectedMedicalRecord = medicalRecord; 
        }
        public void Load()
        {
            patientLabel.Content = _selectedMedicalRecord.Patient.ToString();
            heightLabel.Content = _selectedMedicalRecord.Height.ToString() + " cm";
            weightLabel.Content = _selectedMedicalRecord.Weight.ToString() + " kg";
            foreach (String illness in _selectedMedicalRecord.PreviousIllnesses)
                illnessesListBox.Items.Add(illness);
            foreach (String allergen in _selectedMedicalRecord.Allergens)
                allergensListBox.Items.Add(allergen);
        }
    }
}
