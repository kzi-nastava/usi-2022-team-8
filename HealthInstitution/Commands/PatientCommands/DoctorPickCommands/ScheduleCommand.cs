using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.Core.TrollCounters;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels;
using HealthInstitution.ViewModels.GUIViewModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.DoctorPickCommands;

public class ScheduleCommand : CommandBase
{
    Patient _loggedPatient;
    private DoctorPickViewModel _viewModel;
    ITrollCounterService _trollCounterService;
    public ScheduleCommand(DoctorPickViewModel viewModel,Patient loggedPatient,ITrollCounterService trollCounterService)
    {
        _viewModel = viewModel;
        _loggedPatient = loggedPatient;
        _trollCounterService = trollCounterService;
    }

    public override void Execute(object? parameter)
    {
        try
        {
            _trollCounterService.TrollCheck(_viewModel.LoggedPatient.Username);
            var window = DIContainer.GetService<AddExaminationDialog>();
            var dataContext=window.SetLoggedPatient(_loggedPatient);
            SetSelectedDoctor(dataContext);

            window.ShowDialog();
            _trollCounterService.AppendCreateDates(_viewModel.LoggedPatient.Username);
            MessageBox.Show("Sucessfuly made examination appointment", "Success");
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error");
        }
    }

    private void SetSelectedDoctor(AddExaminationDialogViewModel dataContext)
    {
        for (int i = 0; i < dataContext.DoctorComboBoxItems.Count; i++)
        {
            if (_viewModel.Doctors[_viewModel.SelectedDoctorIndex].Username == dataContext.DoctorComboBoxItems[i])
            {
                dataContext.DoctorComboBoxSelectedIndex = i;
                break;
            }
        }
    }
}