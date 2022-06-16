using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.GUI.DoctorView;
using HealthInstitution.ViewModels.GUIViewModels.DoctorViewViewModels.AppointmentsTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Commands.DoctorCommands.ExaminationPerforming
{
    internal class StartExaminationCommand : CommandBase
    {
        private ScheduledExaminationTableViewModel _scheduledExaminationTableViewModel;
        IExaminationService _examinationService;
        public StartExaminationCommand(ScheduledExaminationTableViewModel scheduledExaminationTableViewModel, IExaminationService examinationService)
        {
            _scheduledExaminationTableViewModel = scheduledExaminationTableViewModel;
            _examinationService = examinationService;
        }

        public override void Execute(object? parameter)
        {
            if (IsExaminationSelected())
            {
                Examination selectedExamination = _scheduledExaminationTableViewModel.GetSelectedExamination();
                if (_examinationService.IsReadyForPerforming(selectedExamination))
                {
                    var window = DIContainer.GetService<PerformExaminationDialog>();
                    window.SetExamination(selectedExamination);
                    window.ShowDialog();
                }                   
                else
                    System.Windows.MessageBox.Show("Date of examination didn't pass!", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            }
            _scheduledExaminationTableViewModel.RefreshGrid();
        }
        private bool IsExaminationSelected()
        {
            if (_scheduledExaminationTableViewModel.GetAppointmentChoice() == 1)
            {
                System.Windows.MessageBox.Show("You have to check examination for it to start!", "Alert", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return false;
            }
            else if (_scheduledExaminationTableViewModel.SelectedExaminationIndex == -1)
            {
                System.Windows.MessageBox.Show("You have to select row to start examination!", "Alert", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
