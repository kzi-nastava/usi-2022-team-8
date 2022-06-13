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
    public class AppointmentNotificationService : IAppointmentNotificationService
    {
        IAppointmentNotificationRepository _appointmentNotificationRepository;
        public AppointmentNotificationService(IAppointmentNotificationRepository appointmentNotificationRepository) {
            _appointmentNotificationRepository = appointmentNotificationRepository;
        }
        public void ChangeActiveStatus(AppointmentNotification notification, bool forDoctor)
        {
            if (forDoctor)
                notification.ActiveForDoctor = false;
            else
                notification.ActiveForPatient = false;
            _appointmentNotificationRepository.Save();
        }
        public void SendNotificationsForDelayedExamination(ScheduleEditRequest selectedAppointment)
        {
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(selectedAppointment.CurrentExamination.Appointment, selectedAppointment.NewExamination.Appointment, selectedAppointment.NewExamination.Doctor, selectedAppointment.NewExamination.MedicalRecord.Patient);
            _appointmentNotificationRepository.Add(appointmentNotificationDto);
        }
        public void SendNotificationForNewExamination(Examination examination)
        {
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(null, examination.Appointment, examination.Doctor, examination.MedicalRecord.Patient);
            _appointmentNotificationRepository.Add(appointmentNotificationDto);
        }
        public void SendNotificationsForDelayedOperation(ScheduleEditRequest selectedAppointment)
        {
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(selectedAppointment.CurrentOperation.Appointment, selectedAppointment.NewOperation.Appointment, selectedAppointment.NewOperation.Doctor, selectedAppointment.NewOperation.MedicalRecord.Patient);
            _appointmentNotificationRepository.Add(appointmentNotificationDto);
        }
        public void SendNotificationForNewOperation(Operation operation)
        {
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(null, operation.Appointment, operation.Doctor, operation.MedicalRecord.Patient);
            _appointmentNotificationRepository.Add(appointmentNotificationDto);
        }
    }
}
