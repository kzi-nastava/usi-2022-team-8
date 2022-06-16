using HealthInstitution.Core;
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
    private DoctorPickViewModel _viewModel;

    public ScheduleCommand(DoctorPickViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter)
    {
        try
        {
            TrollCounterService.TrollCheck(_viewModel.LoggedPatient.Username);
            var window = new AddExaminationDialog(_viewModel.LoggedPatient);
            var dataContex = new AddExaminationDialogViewModel(_viewModel.LoggedPatient);
            SetSelectedDoctor(dataContex);
            window.DataContext = dataContex;
            window.ShowDialog();
            TrollCounterService.AppendCreateDates(_viewModel.LoggedPatient.Username);
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