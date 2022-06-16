using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.MedicalRecords.Repository;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.SystemUsers.Patients.Repository;
using HealthInstitution.Core.SystemUsers.Users.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Scheduling;
using System.Windows;

namespace HealthInstitution.GUI.DoctorView
{
    /// <summary>
    /// Interaction logic for EditOperationDialog.xaml
    /// </summary>
    public partial class EditOperationDialog : Window
    {
        IPatientService _patientService;
        IMedicalRecordService _medicalRecordService;
        IOperationService _operationService;
        Operation _operation;
        public EditOperationDialog(IPatientService patientService, IMedicalRecordService medicalRecordService, IOperationService operationService)
        {
            InitializeComponent();
            _patientService = patientService;
            _medicalRecordService = medicalRecordService;
            _operationService = operationService;
            
        }

        public void SetOperation(Operation operation)
        {
            _operation = operation;
            DataContext = new EditOperationDialogViewModel(operation, _medicalRecordService,_operationService,_patientService);
        }
    }
}
