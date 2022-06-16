using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.SchedulingDialogs
{
    internal class AddExaminationDialogCommand : CommandBase
    {
        private AddExaminationDialogViewModel _addDoctorExaminationDialogViewModel;
        IMedicalRecordService _medicalRecordService;
        ISchedulingService _schedulingService;
        public AddExaminationDialogCommand(AddExaminationDialogViewModel addDoctorExaminationDialogViewModel, IMedicalRecordService medicalRecordService, ISchedulingService schedulingService)
        {
            _addDoctorExaminationDialogViewModel = addDoctorExaminationDialogViewModel;
            _medicalRecordService = medicalRecordService;
            _schedulingService = schedulingService;
        }

        public override void Execute(object? parameter)
        {
            try {
                Doctor doctor = _addDoctorExaminationDialogViewModel.LoggedDoctor;
                Patient patient = _addDoctorExaminationDialogViewModel.GetPatient();
                MedicalRecord medicalRecord = _medicalRecordService.GetByPatientUsername(patient);
                DateTime dateTime = _addDoctorExaminationDialogViewModel.GetExaminationDateTime();
                ExaminationDTO examination = new ExaminationDTO(dateTime, null, doctor, medicalRecord);
                _schedulingService.ReserveExamination(examination);
                System.Windows.MessageBox.Show("You have succesfully added new examination!", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
