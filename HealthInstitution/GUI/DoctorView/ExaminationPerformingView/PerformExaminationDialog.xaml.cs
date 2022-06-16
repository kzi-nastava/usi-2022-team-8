using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Drugs;
using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Prescriptions;
using HealthInstitution.Core.Prescriptions.Model;
using HealthInstitution.Core.Referrals;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.SchedulePerforming;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for PerformExaminationDialog.xaml
    /// </summary>
    public partial class PerformExaminationDialog : Window
    {
        Examination _examination;
        IMedicalRecordService _medicalRecordService;
        IExaminationService _examinationService;
        public PerformExaminationDialog(IExaminationService examinationService,
            IMedicalRecordService medicalRecordService)
        {
            InitializeComponent();
            _medicalRecordService = medicalRecordService;
            _examinationService = examinationService;
        }

        public void SetExamination(Examination examination)
        {
            _examination = examination;
            DataContext = new PerformExaminationDialogViewModel(examination, examination.MedicalRecord,_medicalRecordService,_examinationService);
        }
    }
}
