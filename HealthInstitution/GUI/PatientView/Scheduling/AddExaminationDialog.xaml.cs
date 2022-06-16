using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Users.Model;
using System.Windows;
using System.Windows.Controls;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;

namespace HealthInstitution.GUI.PatientView
{
    /// <summary>
    /// Interaction logic for AddExaminationDialog.xaml
    /// </summary>
    public partial class AddExaminationDialog : Window
    {
        private Patient _loggedPatient;
        IDoctorService _doctorService;
        IMedicalRecordService _medicalRecordService;
        ISchedulingService _schedulingService;

        public AddExaminationDialog(IDoctorService doctorService,
                                    IMedicalRecordService medicalRecordService,
                                    ISchedulingService schedulingService)
        {
            InitializeComponent();
            _doctorService = doctorService;
            _medicalRecordService = medicalRecordService;
            _schedulingService = schedulingService;
        }
        public void SetLoggedPatient(Patient patient)
        {

            _loggedPatient = patient;
            DataContext = new AddExaminationDialogViewModel(patient, _doctorService, _medicalRecordService, _schedulingService);
        }

    }
}