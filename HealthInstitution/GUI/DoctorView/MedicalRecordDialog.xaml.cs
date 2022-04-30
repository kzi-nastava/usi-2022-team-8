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
            PatientLabel.Content = selectedMedicalRecord.patient.ToString();
            HeightLabel.Content = selectedMedicalRecord.height.ToString() + " cm";
            WeightLabel.Content = selectedMedicalRecord.weight.ToString() + " kg";
            foreach (String illness in selectedMedicalRecord.previousIllnesses)
                IllnessesListBox.Items.Add(illness);
            foreach (String allergen in selectedMedicalRecord.allergens)
                AllergensListBox.Items.Add(allergen);
        }
    }
}
