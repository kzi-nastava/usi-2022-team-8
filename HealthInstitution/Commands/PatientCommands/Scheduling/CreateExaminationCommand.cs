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
    private IMedicalRecordService _medicalRecordService;
    private IDoctorService _doctorService;
    private ISchedulingService _schedulingService;

    public CreateExaminationCommand(AddExaminationDialogViewModel addExaminationDialogViewModel, IMedicalRecordService medicalRecordService, IDoctorService doctorService, ISchedulingService schedulingService)
    {
        _addExaminationDialogViewModel = addExaminationDialogViewModel;
        _medicalRecordService = medicalRecordService;
        _doctorService = doctorService;
        _schedulingService = schedulingService;
    }

    public override void Execute(object? parameter)
    {
        try
        {
            MedicalRecord medicalRecord = _medicalRecordService.GetByPatientUsername(_addExaminationDialogViewModel.LoggedPatient);
            Doctor doctor = _doctorService.GetById(_addExaminationDialogViewModel.GetDoctorUsername());
            DateTime dateTime = _addExaminationDialogViewModel.GetExaminationDateTime();
            ExaminationDTO examination = new ExaminationDTO(dateTime, null, doctor, medicalRecord);
            _schedulingService.ReserveExamination(examination);
            _addExaminationDialogViewModel.ThisWindow.Close();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error");
        }
    }
}