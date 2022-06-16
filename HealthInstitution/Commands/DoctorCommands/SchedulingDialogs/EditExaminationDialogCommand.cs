using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Scheduling;
using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Examinations;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.SchedulingDialogs
{
    internal class EditExaminationDialogCommand : CommandBase
    {
        private EditExaminationDialogViewModel _editExaminationDialogViewModel;
        IMedicalRecordService _medicalRecordService;
        IExaminationService _examinationService;
        public EditExaminationDialogCommand(EditExaminationDialogViewModel editExaminationDialogViewModel, IMedicalRecordService medicalRecordService, IExaminationService examinationService)
        {
            _editExaminationDialogViewModel = editExaminationDialogViewModel;
            _medicalRecordService = medicalRecordService;
            _examinationService = examinationService;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                var selectedExamination = _editExaminationDialogViewModel.SelectedExamination;
                Doctor doctor = selectedExamination.Doctor;
                Patient patient = _editExaminationDialogViewModel.GetPatient();
                MedicalRecord medicalRecord = _medicalRecordService.GetByPatientUsername(patient);
                DateTime dateTime = _editExaminationDialogViewModel.GetExaminationDateTime();
                ExaminationDTO examination = new ExaminationDTO(dateTime, null, doctor, medicalRecord);
                _examinationService.Update(selectedExamination.Id, examination);
                System.Windows.MessageBox.Show("You have succesfully edited examination!", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
