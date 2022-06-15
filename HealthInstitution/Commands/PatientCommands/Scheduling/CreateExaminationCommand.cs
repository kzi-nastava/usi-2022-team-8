using HealthInstitution.Core;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.Scheduling;

internal class CreateExaminationCommand : CommandBase
{
    private AddExaminationDialogViewModel _addExaminationDialogViewModel;

    public CreateExaminationCommand(AddExaminationDialogViewModel addExaminationDialogViewModel)
    {
        _addExaminationDialogViewModel = addExaminationDialogViewModel;
    }

    public override void Execute(object? parameter)
    {
        MedicalRecord medicalRecord = MedicalRecordService.GetByPatientUsername(_addExaminationDialogViewModel.LoggedPatient);
        Doctor doctor = DoctorService.GetById(_addExaminationDialogViewModel.GetDoctorUsername());
        DateTime dateTime = _addExaminationDialogViewModel.GetExaminationDateTime();
        ExaminationDTO examination = new ExaminationDTO(dateTime, null, doctor, medicalRecord);
        SchedulingService.ReserveExamination(examination);
    }
}