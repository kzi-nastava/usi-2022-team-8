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
        public MedicalRecordDialog(MedicalRecord medicalRecord)
        {
            InitializeComponent();
            DataContext = new MedicalRecordDialogViewModel(medicalRecord);
        }
    }
}
