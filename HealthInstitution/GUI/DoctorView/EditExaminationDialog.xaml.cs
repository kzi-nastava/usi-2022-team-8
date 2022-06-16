using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Scheduling;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for EditExaminationDialog.xaml
    /// </summary>
    public partial class EditExaminationDialog : Window
    {
        IPatientService _patientService;
        IMedicalRecordService _medicalRecordService;
        IExaminationService _examinationService;
        Examination _examination;
        public EditExaminationDialog(IPatientService patientService, IMedicalRecordService medicalRecordService, IExaminationService examinationService)
        {
            InitializeComponent();
            _patientService = patientService;
            _medicalRecordService = medicalRecordService;
            _examinationService = examinationService;           
        }

        public void SetExamination(Examination examination)
        {
            _examination = examination;
            DataContext = new EditExaminationDialogViewModel(examination, _medicalRecordService, _patientService, _examinationService);
        }
    }
}
