using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Notifications.Model;
using HealthInstitution.Core.Notifications.Repository;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using HealthInstitution.Core.SystemUsers.Patients;
using HealthInstitution.Core.SystemUsers.Patients.Model;


namespace HealthInstitution.Core.Scheduling
{
    internal static class UrgentService
    {
        public static List<ScheduleEditRequest> PrepareDataForDelaying(List<Tuple<int, int, DateTime>> examinationsAndOperationsForDelaying)
        {
            List<ScheduleEditRequest> delayedAppointments = new List<ScheduleEditRequest>();
            foreach (Tuple<int, int, DateTime> tuple in examinationsAndOperationsForDelaying)
            {
                if (tuple.Item2 == 1)
                {
                    Examination currentExamination = ExaminationRepository.GetInstance().GetById(tuple.Item1);
                    Examination newExamination = new Examination(currentExamination.Id, ExaminationStatus.Scheduled, tuple.Item3, currentExamination.Room, currentExamination.Doctor, currentExamination.MedicalRecord, "");
                    delayedAppointments.Add(new ScheduleEditRequest(0, currentExamination, newExamination, Core.RestRequests.Model.RestRequestState.OnHold));
                }
                if (tuple.Item2 == 0)
                {
                    Operation currentOperation = OperationRepository.GetInstance().GetById(tuple.Item1);
                    Operation newOperation = new Operation(currentOperation.Id, tuple.Item3, currentOperation.Duration, currentOperation.Room, currentOperation.Doctor, currentOperation.MedicalRecord);
                    delayedAppointments.Add(new ScheduleEditRequest(0, currentOperation, newOperation, Core.RestRequests.Model.RestRequestState.OnHold));
                }
            }
            return delayedAppointments;
        }
        public static void DelayExamination(ScheduleEditRequest selectedAppointment, Examination examination)
        {
            ExaminationRepository.GetInstance().SwapExaminationValue(selectedAppointment.NewExamination);
            SetExaminationDetails(examination, selectedAppointment);
            ExaminationDTO examinationDTO = new ExaminationDTO(examination.Appointment, examination.Room, examination.Doctor, examination.MedicalRecord);
            ExaminationService.Add(examinationDTO);
            SendNotificationsForExamination(examination, selectedAppointment);
        }
        public static void DelayOperation(ScheduleEditRequest selectedAppointment, Operation operation)
        {
            OperationRepository.GetInstance().SwapOperationValue(selectedAppointment.NewOperation);
            SetOperationDetails(operation, selectedAppointment);
            OperationDTO operationDTO = new OperationDTO(operation.Appointment, operation.Duration, operation.Room, operation.Doctor, operation.MedicalRecord);
            OperationService.Add(operationDTO);
            SendNotificationsForOperation(operation, selectedAppointment);
        }
        private static void SendNotificationsForExamination(Examination examination, ScheduleEditRequest selectedAppointment)
        {
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(selectedAppointment.CurrentExamination.Appointment, selectedAppointment.NewExamination.Appointment, selectedAppointment.NewExamination.Doctor, selectedAppointment.NewExamination.MedicalRecord.Patient);
            AppointmentNotificationRepository.GetInstance().Add(appointmentNotificationDto);
            appointmentNotificationDto = new AppointmentNotificationDTO(null, examination.Appointment, examination.Doctor, examination.MedicalRecord.Patient);
            AppointmentNotificationRepository.GetInstance().Add(appointmentNotificationDto);
        }
        private static void SendNotificationsForOperation(Operation operation, ScheduleEditRequest selectedAppointment)
        {
            AppointmentNotificationDTO appointmentNotificationDto = new AppointmentNotificationDTO(selectedAppointment.CurrentOperation.Appointment, selectedAppointment.NewOperation.Appointment, selectedAppointment.NewOperation.Doctor, selectedAppointment.NewOperation.MedicalRecord.Patient);
            AppointmentNotificationRepository.GetInstance().Add(appointmentNotificationDto);
            appointmentNotificationDto = new AppointmentNotificationDTO(null, operation.Appointment, operation.Doctor, operation.MedicalRecord.Patient);
            AppointmentNotificationRepository.GetInstance().Add(appointmentNotificationDto);
        }
        
        private static void SetExaminationDetails(Examination examination, ScheduleEditRequest selectedAppointment)
        {
            if (selectedAppointment.CurrentExamination == null)
            {
                examination.Appointment = selectedAppointment.CurrentOperation.Appointment;
                examination.Room = selectedAppointment.CurrentOperation.Room;
                examination.Doctor = selectedAppointment.CurrentOperation.Doctor;
            }
            else
            {
                examination.Appointment = selectedAppointment.CurrentExamination.Appointment;
                examination.Room = selectedAppointment.CurrentExamination.Room;
                examination.Doctor = selectedAppointment.CurrentExamination.Doctor;
            }
        }
        private static void SetOperationDetails(Operation operation, ScheduleEditRequest selectedAppointment)
        {
            if (selectedAppointment.CurrentExamination == null)
            {
                operation.Appointment = selectedAppointment.CurrentOperation.Appointment;
                operation.Room = selectedAppointment.CurrentOperation.Room;
                operation.Doctor = selectedAppointment.CurrentOperation.Doctor;
            }
            else
            {
                operation.Appointment = selectedAppointment.CurrentExamination.Appointment;
                operation.Room = selectedAppointment.CurrentExamination.Room;
                operation.Doctor = selectedAppointment.CurrentExamination.Doctor;
            }
        }

        private static void TrySchedulingUrgentOperation(DateTime appointment, int duration, Doctor doctor, MedicalRecord medicalRecord, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
        {
            OperationDTO operationDTO = new OperationDTO(appointment, duration, null, doctor, medicalRecord);
            DoctorOperationAvailabilityService.CheckIfDoctorIsAvailable(operationDTO);
            PatientOperationAvailabilityService.CheckIfPatientIsAvailable(operationDTO);
            operationDTO.Room = RoomService.FindAvailableRoom(operationDTO);
            OperationService.Add(operationDTO);
            AppointmentNotificationDTO appointmentNotificationDTO = new AppointmentNotificationDTO(null, appointment, doctor, medicalRecord.Patient);
            AppointmentNotificationRepository.GetInstance().Add(appointmentNotificationDTO);
            priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(OperationRepository.GetInstance()._maxId, 2, appointment));
        }
        public static List<Tuple<int, int, DateTime>> ReserveUrgentOperation(string patientUsername, SpecialtyType specialtyType, int duration)
        {
            List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
            Patient patient = PatientService.GetByUsername(patientUsername);
            var medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
            List<DateTime> nextTwoHoursAppointments = AppointmentDelayingService.FindNextTwoHoursAppointments();
            foreach (DateTime appointment in nextTwoHoursAppointments)
            {
                foreach (Doctor doctor in DoctorService.GetAll())
                {
                    if (doctor.Specialty == specialtyType)
                    {
                        try
                        {
                            TrySchedulingUrgentOperation(appointment, duration, doctor, medicalRecord, priorityExaminationsAndOperations);
                            return priorityExaminationsAndOperations;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(OperationRepository.GetInstance()._maxId + 1, 2, new DateTime(1, 1, 1)));
            priorityExaminationsAndOperations.AddRange(AppointmentDelayingService.FindClosest(nextTwoHoursAppointments, specialtyType));
            return priorityExaminationsAndOperations;
        }

        public static List<Tuple<int, int, DateTime>> ReserveUrgentExamination(string patientUsername, SpecialtyType specialtyType)
        {
            List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
            Patient patient = PatientService.GetByUsername(patientUsername);
            var medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
            List<DateTime> nextTwoHoursAppointments = AppointmentDelayingService.FindNextTwoHoursAppointments();
            foreach (DateTime appointment in nextTwoHoursAppointments)
            {
                foreach (Doctor doctor in DoctorService.GetAll())
                {
                    if (doctor.Specialty == specialtyType)
                    {
                        try
                        {
                            TrySchedulingUrgentExamination(appointment, doctor, medicalRecord, priorityExaminationsAndOperations);
                            return priorityExaminationsAndOperations;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            int id = ExaminationRepository.GetInstance()._maxId;
            priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(id + 1, 2, new DateTime(1, 1, 1)));
            priorityExaminationsAndOperations.AddRange(AppointmentDelayingService.FindClosest(nextTwoHoursAppointments, specialtyType));
            return priorityExaminationsAndOperations;
        }

        private static void TrySchedulingUrgentExamination(DateTime appointment, Doctor doctor, MedicalRecord medicalRecord, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
        {
            ExaminationDTO examinationDTO = new ExaminationDTO(appointment, null, doctor, medicalRecord);
            DoctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
            PatientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
            examinationDTO.Room = RoomService.FindAvailableRoom(appointment);
            ExaminationService.Add(examinationDTO);
            AppointmentNotificationDTO appointmentNotificationDTO = new AppointmentNotificationDTO(null, appointment, doctor, medicalRecord.Patient);
            AppointmentNotificationRepository.GetInstance().Add(appointmentNotificationDTO);
            int id = ExaminationRepository.GetInstance()._maxId;
            priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(id, 2, appointment));
        }
    }
}
