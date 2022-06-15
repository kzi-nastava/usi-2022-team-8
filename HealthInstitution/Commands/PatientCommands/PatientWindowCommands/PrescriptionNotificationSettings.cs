using HealthInstitution.Core;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.PrescriptionNotificationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.PatientWindowCommands
{
    public class PrescriptionNotificationSettingsCommand : CommandBase
    {
        private string _username;

        public PrescriptionNotificationSettingsCommand(string username)
        {
            _username = username;
        }

        public override void Execute(object? parameter)
        {
            new RecepieNotificationSettingsDialog(_username)
            {
                DataContext = new PrescriptionNotificationSettingViewModel(PatientService.GetByUsername(_username))
            }.ShowDialog();
        }
    }
}