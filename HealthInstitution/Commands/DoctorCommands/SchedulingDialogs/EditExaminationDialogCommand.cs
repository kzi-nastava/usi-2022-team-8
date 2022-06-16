using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Scheduling;
using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.Examinations;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.Scheduling
{
    internal class EditExaminationDialogCommand : CommandBase
    {
        private EditExaminationDialogViewModel _editExaminationDialogViewModel;

        public EditExaminationDialogCommand(EditExaminationDialogViewModel editExaminationDialogViewModel)
        {
            _editExaminationDialogViewModel = editExaminationDialogViewModel;
        }

        public override void Execute(object? parameter)
        {
            try
            {
                var selectedExamination = _editExaminationDialogViewModel.SelectedExamination;
                Doctor doctor = selectedExamination.Doctor;
                Patient patient = _editExaminationDialogViewModel.GetPatient();
                MedicalRecord medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
                DateTime dateTime = _editExaminationDialogViewModel.GetExaminationDateTime();
                ExaminationDTO examination = new ExaminationDTO(dateTime, null, doctor, medicalRecord);
                ExaminationService.Update(selectedExamination.Id, examination);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
