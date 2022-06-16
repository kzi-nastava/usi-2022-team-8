using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Scheduling;
using HealthInstitution.Core;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Operations;
using System.Windows;


namespace HealthInstitution.Commands.DoctorCommands.SchedulingDialogs
{
    public class EditOperationDialogCommand : CommandBase
    {
        private EditOperationDialogViewModel _editOperationDialogViewModel;
        IMedicalRecordService _medicalRecordService;
        IOperationService _operationService;
        public EditOperationDialogCommand(EditOperationDialogViewModel editOperationDialogViewModel, IMedicalRecordService medicalRecordService, IOperationService operationService)
        {
            _editOperationDialogViewModel = editOperationDialogViewModel;
            _medicalRecordService = medicalRecordService;
            _operationService = operationService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                var selectedOperation = _editOperationDialogViewModel.SelectedOperation;
                Doctor doctor = selectedOperation.Doctor;
                Patient patient = _editOperationDialogViewModel.GetPatient();
                MedicalRecord medicalRecord = _medicalRecordService.GetByPatientUsername(patient);
                DateTime dateTime = _editOperationDialogViewModel.GetOperationDateTime();
                int duration = _editOperationDialogViewModel.GetDuration();
                OperationDTO operation = new OperationDTO(dateTime, duration, null, doctor, medicalRecord);
                _operationService.Update(selectedOperation.Id, operation);
                System.Windows.MessageBox.Show("You have succesfully added edited operation!", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
