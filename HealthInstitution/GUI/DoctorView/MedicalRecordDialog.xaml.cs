using HealthInstitution.Core.MedicalRecords.Model;
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
    /// Interaction logic for MedicalRecordDialog.xaml
    /// </summary>
    public partial class MedicalRecordDialog : Window
    {
        public MedicalRecordDialog(MedicalRecord selectedMedicalRecord)
        {
            InitializeComponent();
            patientLabel.Content = selectedMedicalRecord.Patient.ToString();
            heightLabel.Content = selectedMedicalRecord.Height.ToString() + " cm";
            weightLabel.Content = selectedMedicalRecord.Weight.ToString() + " kg";
            foreach (String illness in selectedMedicalRecord.PreviousIllnesses)
                illnessesListBox.Items.Add(illness);
            foreach (String allergen in selectedMedicalRecord.Allergens)
                allergensListBox.Items.Add(allergen);
        }
    }
}
