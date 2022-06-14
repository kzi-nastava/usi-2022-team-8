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
        private void Add(AppointmentNotificationDTO appointmentNotificationDTO)
        {
            AppointmentNotification appointmentNotification = new AppointmentNotification(appointmentNotificationDTO);
            _appointmentNotificationRepository.Add(appointmentNotification);
        }
        public void SendNotificationsForDelayedExamination(ScheduleEditRequest selectedAppointment)
        {
            AppointmentNotificationDTO appointmentNotificationDTO = new AppointmentNotificationDTO(selectedAppointment.CurrentExamination.Appointment, selectedAppointment.NewExamination.Appointment, selectedAppointment.NewExamination.Doctor, selectedAppointment.NewExamination.MedicalRecord.Patient);
            Add(appointmentNotificationDTO);
        }
        public void SendNotificationForNewExamination(Examination examination)
        {
            AppointmentNotificationDTO appointmentNotificationDTO = new AppointmentNotificationDTO(null, examination.Appointment, examination.Doctor, examination.MedicalRecord.Patient);
            Add(appointmentNotificationDTO);
        }
        public void SendNotificationsForDelayedOperation(ScheduleEditRequest selectedAppointment)
        {
            AppointmentNotificationDTO appointmentNotificationDTO = new AppointmentNotificationDTO(selectedAppointment.CurrentOperation.Appointment, selectedAppointment.NewOperation.Appointment, selectedAppointment.NewOperation.Doctor, selectedAppointment.NewOperation.MedicalRecord.Patient);
            Add(appointmentNotificationDTO);
        }
        public void SendNotificationForNewOperation(Operation operation)
        {
            AppointmentNotificationDTO appointmentNotificationDTO = new AppointmentNotificationDTO(null, operation.Appointment, operation.Doctor, operation.MedicalRecord.Patient);
            Add(appointmentNotificationDTO);
        }
    }
}
