using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.ScheduleEditRequests;
using HealthInstitution.Core.Scheduling;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.PatientCommands.Scheduling;

public class EditExaminationCommand : Core.CommandBase
{
    private EditExaminationDialogViewModel _editExaminationDialogViewModel;
    private Examination _selectedExamination;
    IExaminationService _examinationService;
    IEditSchedulingService _editSchedulingService;
    IScheduleEditRequestsService _scheduleEditRequestsService;
    IDoctorService _doctorService;
    public EditExaminationCommand(EditExaminationDialogViewModel editExaminationDialogViewModel, Examination selectedExamination,IExaminationService examinationService,IEditSchedulingService editSchedulingService,IScheduleEditRequestsService scheduleEditRequestsService,IDoctorService doctorService)
    {
        _editExaminationDialogViewModel = editExaminationDialogViewModel;
        _selectedExamination = selectedExamination;
        _examinationService = examinationService;
        _editSchedulingService = editSchedulingService;
        _scheduleEditRequestsService = scheduleEditRequestsService;
        _doctorService = doctorService;
    }

    public override void Execute(object? parameter)
    {
        DateTime dateTime = _editExaminationDialogViewModel.GetExaminationDateTime();
        try
        {
            if (_selectedExamination.Appointment.AddDays(-2) < DateTime.Today)
            {
                GenerateRequest(dateTime);
            }
            else
            {
                EditNow(dateTime);
            }
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "Question", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void GenerateRequest(DateTime dateTime)
    {
        ExaminationDTO examinationDTO = _examinationService.ParseExaminationToExaminationDTO(_selectedExamination);
        examinationDTO.Doctor = _doctorService.GetById(_editExaminationDialogViewModel.GetDoctorUsername());
        examinationDTO.Appointment = dateTime;
        Examination newExamination = _editSchedulingService.GenerateRequestExamination(_selectedExamination.Id, examinationDTO);
        _scheduleEditRequestsService.AddEditRequest(newExamination);
    }

    private void EditNow(DateTime dateTime)
    {
        ExaminationDTO examinationDTO = _examinationService.ParseExaminationToExaminationDTO(_selectedExamination);
        examinationDTO.Doctor = _doctorService.GetById(_editExaminationDialogViewModel.GetDoctorUsername());
        examinationDTO.Appointment = dateTime;
        _editSchedulingService.EditExamination(_selectedExamination.Id, examinationDTO);
    }
}