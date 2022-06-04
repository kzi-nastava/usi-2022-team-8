using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Notifications
{
    public static class AppointmentNotificationService
    {
        private static AppointmentNotificationRepository s_appointmentNotificationRepository = AppointmentNotificationRepository.GetInstance();
        public static void ChangeActiveStatus(AppointmentNotification notification, bool forDoctor)
        {
            if (forDoctor)
                notification.ActiveForDoctor = false;
            else
                notification.ActiveForPatient = false;
            s_appointmentNotificationRepository.Save();
        }
        public static void SendNotificationsForDelayedExamination(ScheduleEditRequest selectedAppointment)
        {
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(selectedAppointment.CurrentExamination.Appointment, selectedAppointment.NewExamination.Appointment, selectedAppointment.NewExamination.Doctor, selectedAppointment.NewExamination.MedicalRecord.Patient);
            s_appointmentNotificationRepository.Add(appointmentNotificationDto);
        }
        public static void SendNotificationForNewExamination(Examination examination)
        {
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(null, examination.Appointment, examination.Doctor, examination.MedicalRecord.Patient);
            s_appointmentNotificationRepository.Add(appointmentNotificationDto);
        }
        public static void SendNotificationsForDelayedOperation(ScheduleEditRequest selectedAppointment)
        {
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(selectedAppointment.CurrentOperation.Appointment, selectedAppointment.NewOperation.Appointment, selectedAppointment.NewOperation.Doctor, selectedAppointment.NewOperation.MedicalRecord.Patient);
            s_appointmentNotificationRepository.Add(appointmentNotificationDto);
        }
        public static void SendNotificationForNewOperation(Operation operation)
        {
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(null, operation.Appointment, operation.Doctor, operation.MedicalRecord.Patient);
            s_appointmentNotificationRepository.Add(appointmentNotificationDto);
        }
    }
}
