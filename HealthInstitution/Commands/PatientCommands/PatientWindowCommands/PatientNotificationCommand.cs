using HealthInstitution.Core;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.SystemUsers.Patients.Model;
using HealthInstitution.GUI.PatientView;
using HealthInstitution.ViewModels.GUIViewModels.PatientViewViewModels.PrescriptionNotificationViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Commands.PatientCommands.PatientWindowCommands
{
    public class PatientNotificationCommand : CommandBase
    {
        private Patient _loggedPatient;

        public PatientNotificationCommand(Patient loggedPatient)
        {
            _loggedPatient = loggedPatient;
        }

        public override void Execute(object? parameter)
        {
            int activeNotifications = 0;
            foreach (AppointmentNotification notification in this._loggedPatient.Notifications)
            {
                if (notification.ActiveForPatient)
                    activeNotifications++;
            }
            if (activeNotifications > 0)
            {
                PatientNotificationsDialog patientNotificationsDialog = new PatientNotificationsDialog(this._loggedPatient);
                patientNotificationsDialog.ShowDialog();
            }
            new RecepieNotificationDialog(_loggedPatient.Username)
            {
                DataContext = new PrescriptionNotificationDialogViewModel(_loggedPatient)
            }.ShowDialog();
        }
    }
}