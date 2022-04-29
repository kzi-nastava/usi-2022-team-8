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
        MedicalRecord medicalRecord;
        public MedicalRecordDialog(MedicalRecord medicalRecord)
        {
            /*medicalRecord = selectedMedicalRecord;
            PatientLabel.Content = medicalRecord.patient.ToString();
            HeightLabel.Content = medicalRecord.height.ToString();
            WeightLabel.Content = medicalRecord.weight.ToString();
            IllnessesListBox.DataContext = medicalRecord.previousIllnesses;
            AllergensListBox.DataContext = medicalRecord.allergens;*/
            medicalRecord = medicalRecord;
            InitializeComponent();
        }

       /* [STAThread]
        static void Main(string[] args)
        {
            MedicalRecordDialog window = new MedicalRecordDialog();
            window.ShowDialog();
        }*/
    }
}
