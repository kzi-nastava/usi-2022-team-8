using HealthInstitution.Core.Examinations.Model;
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
    /// Interaction logic for PerformExaminationDialog.xaml
    /// </summary>
    public partial class PerformExaminationDialog : Window
    {
        public Examination examination { get; set; }
        public PerformExaminationDialog(Examination selectedExamination)
        {
            InitializeComponent();
            examination = selectedExamination;
            MedicalRecord medicalRecord = examination.medicalRecord;
            PatientTextBox.Text = medicalRecord.patient.ToString();
            HeightTextBox.Text = medicalRecord.height.ToString();
            WeightTextBox.Text = medicalRecord.weight.ToString();
            IllnessListBox.DataContext = medicalRecord.previousIllnesses;
            AllergenListBox.DataContext = medicalRecord.allergens;
        }
/*
        [STAThread]
        static void Main(string[] args)
        {
            PerformExaminationDialog window = new PerformExaminationDialog();
            window.ShowDialog();
        }
*/
        private void AddIllness_Click(object sender, RoutedEventArgs e)
        {
            IllnessListBox.Items.Add(IllnessTextBox.Text);
            IllnessListBox.Items.Refresh();
        }

        private void AddAllergen_Click(object sender, RoutedEventArgs e)
        {
            AllergenListBox.Items.Add(AllergenTextBox.Text);
            AllergenListBox.Items.Refresh();
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
