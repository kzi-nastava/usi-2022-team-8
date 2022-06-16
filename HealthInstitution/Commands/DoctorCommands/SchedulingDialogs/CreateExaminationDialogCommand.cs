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

namespace HealthInstitution.Commands.DoctorCommands.Scheduling
{
    internal class CreateExaminationDialogCommand : CommandBase
    {
        private AddExaminationDialogViewModel _addDoctorExaminationDialogViewModel;

        public CreateExaminationDialogCommand(AddExaminationDialogViewModel addDoctorExaminationDialogViewModel)
        {
            _addDoctorExaminationDialogViewModel = addDoctorExaminationDialogViewModel;
        }

        public override void Execute(object? parameter)
        {
            Doctor doctor = _addDoctorExaminationDialogViewModel.LoggedDoctor;
            Patient patient = _addDoctorExaminationDialogViewModel.GetPatient();
            MedicalRecord medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
            DateTime dateTime = _addDoctorExaminationDialogViewModel.GetExaminationDateTime();
            ExaminationDTO examination = new ExaminationDTO(dateTime, null, doctor, medicalRecord);
            SchedulingService.ReserveExamination(examination);
        }
    }
}
