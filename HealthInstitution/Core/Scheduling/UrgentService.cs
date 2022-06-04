using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.MedicalRecords;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Notifications;
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
    public static class UrgentService
    {
        public static void SetExaminationDetails(Examination examination, ScheduleEditRequest selectedAppointment)
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
        public static void SetOperationDetails(Operation operation, ScheduleEditRequest selectedAppointment)
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
        private static List<DateTime> FindNextTwoHoursAppointments()
        {
            List<DateTime> possibleAppointments = new List<DateTime>();
            DateTime current = DateTime.Now;
            DateTime firstAppointment = current;

            if (current.Minute > 0) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 15, 0);
            if (current.Minute > 15) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 30, 0);
            if (current.Minute > 30) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 45, 0);
            if (current.Minute > 45) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour + 1, 0, 0);

            for (int i = 0; i <= 7; i++)
            {
                TimeSpan ts = new TimeSpan(0, 15, 0);
                possibleAppointments.Add(firstAppointment + i * ts);
            }
            return possibleAppointments;
        }
        private static void TrySchedulingUrgentOperation(DateTime appointment, int duration, Doctor doctor, MedicalRecord medicalRecord, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
        {
            OperationDTO operationDTO = new OperationDTO(appointment, duration, null, doctor, medicalRecord);
            DoctorOperationAvailabilityService.CheckIfDoctorIsAvailable(operationDTO);
            PatientOperationAvailabilityService.CheckIfPatientIsAvailable(operationDTO);
            operationDTO.Room = SchedulingService.FindAvailableOperationRoom(operationDTO);
            Operation operation=OperationService.Add(operationDTO);
            AppointmentNotificationService.SendNotificationForNewOperation(operation);
            int id = OperationRepository.GetInstance()._maxId;
            priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(id, 2, appointment));
        }
        public static List<Tuple<int, int, DateTime>> ReserveUrgentOperation(string patientUsername, SpecialtyType specialtyType, int duration)
        {
            List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
            Patient patient = PatientService.GetByUsername(patientUsername);
            var medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
            List<DateTime> nextTwoHoursAppointments = FindNextTwoHoursAppointments();
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
            priorityExaminationsAndOperations.AddRange(AppointmentDelayingService.FindClosest(nextTwoHoursAppointments, specialtyType, Rooms.Model.RoomType.OperatingRoom));
            return priorityExaminationsAndOperations;
        }
        private static void TrySchedulingUrgentExamination(DateTime appointment, Doctor doctor, MedicalRecord medicalRecord, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
        {
            ExaminationDTO examinationDTO = new ExaminationDTO(appointment, null, doctor, medicalRecord);
            DoctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
            PatientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
            examinationDTO.Room = SchedulingService.FindAvailableExaminationRoom(appointment);
            Examination examination=ExaminationService.Add(examinationDTO);
            AppointmentNotificationService.SendNotificationForNewExamination(examination);
            int id = ExaminationRepository.GetInstance()._maxId;
            priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(id, 2, appointment));
        }
        public static List<Tuple<int, int, DateTime>> ReserveUrgentExamination(string patientUsername, SpecialtyType specialtyType)
        {
            List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
            Patient patient = PatientService.GetByUsername(patientUsername);
            var medicalRecord = MedicalRecordService.GetByPatientUsername(patient);
            List<DateTime> nextTwoHoursAppointments = FindNextTwoHoursAppointments();
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
            priorityExaminationsAndOperations.AddRange(AppointmentDelayingService.FindClosest(nextTwoHoursAppointments, specialtyType, Rooms.Model.RoomType.ExaminationRoom));
            return priorityExaminationsAndOperations;
        }
    }
}
