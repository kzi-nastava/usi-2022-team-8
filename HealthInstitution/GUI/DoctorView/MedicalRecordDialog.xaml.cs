using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.SchedulePerforming;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for MedicalRecordDialog.xaml
    /// </summary>
    public partial class MedicalRecordDialog : Window
    {
        MedicalRecord _medicalRecord;
        public MedicalRecordDialog()
        {
            InitializeComponent();            
        }

        public void SetMedicalRecord(MedicalRecord medicalRecord)
        {
            _medicalRecord = medicalRecord;
            DataContext = new MedicalRecordDialogViewModel(medicalRecord);
        }
    }
}
