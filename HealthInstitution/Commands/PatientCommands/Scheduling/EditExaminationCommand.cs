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

    public EditExaminationCommand(EditExaminationDialogViewModel editExaminationDialogViewModel, Examination selectedExamination)
    {
        _editExaminationDialogViewModel = editExaminationDialogViewModel;
        _selectedExamination = selectedExamination;
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
        ExaminationDTO examinationDTO = ExaminationService.ParseExaminationToExaminationDTO(_selectedExamination);
        examinationDTO.Doctor = DoctorService.GetById(_editExaminationDialogViewModel.GetDoctorUsername());
        examinationDTO.Appointment = dateTime;
        Examination newExamination = EditSchedulingService.GenerateRequestExamination(_selectedExamination.Id, examinationDTO);
        ScheduleEditRequestService.AddEditRequest(newExamination);
    }

    private void EditNow(DateTime dateTime)
    {
        ExaminationDTO examinationDTO = ExaminationService.ParseExaminationToExaminationDTO(_selectedExamination);
        examinationDTO.Doctor = DoctorService.GetById(_editExaminationDialogViewModel.GetDoctorUsername());
        examinationDTO.Appointment = dateTime;
        EditSchedulingService.EditExamination(_selectedExamination.Id, examinationDTO);
    }
}