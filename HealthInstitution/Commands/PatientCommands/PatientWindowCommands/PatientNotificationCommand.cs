using HealthInstitution.Core;
using HealthInstitution.Core.DIContainer;
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
                var patientNotificationWindow = DIContainer.GetService<PatientNotificationsDialog>();
                patientNotificationWindow.SetLoggedPatient(_loggedPatient);
                patientNotificationWindow.ShowDialog();
            }
            var prescriptionNotificationWindow = DIContainer.GetService<RecepieNotificationDialog>();
            prescriptionNotificationWindow.SetLoggedPatient(_loggedPatient);
            prescriptionNotificationWindow.ShowDialog();
        }
    }
}