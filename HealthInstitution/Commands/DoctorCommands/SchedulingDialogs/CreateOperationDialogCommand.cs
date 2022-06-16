using System;
using HealthInstitution.Core;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Scheduling;

namespace HealthInstitution.Commands.DoctorCommands.SchedulingDialogs
{
    internal class CreateOperationDialogCommand : CommandBase
    {
        private AddOperationDialogViewModel _addOperationDialogViewModel;

        public CreateOperationDialogCommand(AddOperationDialogViewModel addOperationDialogViewModel)
        {
            _addOperationDialogViewModel = addOperationDialogViewModel;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                Doctor doctor = _addOperationDialogViewModel.LoggedDoctor;
                Patient patient = _addOperationDialogViewModel.GetPatient();
                MedicalRecord medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
                DateTime dateTime = _addOperationDialogViewModel.GetOperationDateTime();
                int duration = _addOperationDialogViewModel.GetDuration();
                OperationDTO operation = new OperationDTO(dateTime, duration, null, doctor, medicalRecord);
                SchedulingService.ReserveOperation(operation);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
