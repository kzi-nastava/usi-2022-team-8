using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Notifications
{
    public interface IAppointmentNotificationService
    {
        public void ChangeActiveStatus(AppointmentNotification notification, bool forDoctor);
        public void SendNotificationsForDelayedExamination(ScheduleEditRequest selectedAppointment);
        public void SendNotificationForNewExamination(Examination examination);
        public void SendNotificationsForDelayedOperation(ScheduleEditRequest selectedAppointment);
        public void SendNotificationForNewOperation(Operation operation);
    }
}
