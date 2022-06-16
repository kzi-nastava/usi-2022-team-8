using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Scheduling;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for AddExaminationDialog.xaml
    /// </summary>
    public partial class AddExaminationDialog : Window
    {
        IPatientService _patientService;
        IMedicalRecordService _medicalRecordService;
        ISchedulingService _schedulingService;
        Doctor _loggedDoctor;
        public AddExaminationDialog(IPatientService patientService,
                                    IMedicalRecordService medicalRecordService,
                                    ISchedulingService schedulingService)
        {
            InitializeComponent();
            _patientService = patientService;
            _medicalRecordService = medicalRecordService;
            _schedulingService = schedulingService;
        }

        public void SetLoggedDoctor(Doctor doctor)
        {
            _loggedDoctor = doctor;
            DataContext = new AddExaminationDialogViewModel(doctor,_patientService,_medicalRecordService,_schedulingService);
        }
    }
}
