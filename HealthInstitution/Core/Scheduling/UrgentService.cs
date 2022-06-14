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
    public class UrgentService : IUrgentService
    {
        IDoctorOperationAvailabilityService _doctorOperationAvailabilityService;
        IPatientOperationAvailabilityService _patientOperationAvailabilityService;
        IDoctorExaminationAvailabilityService _doctorExaminationAvailabilityService;
        IPatientExaminationAvailabilityService _patientExaminationAvailabilityService;
        ISchedulingService _schedulingService;
        IOperationService _operationService;
        IExaminationService _examinationService;
        IPatientService _patientService;
        IDoctorService _doctorService;
        IMedicalRecordService _medicalRecordService;
        IAppointmentNotificationService _appointmentNotificationService;
        IAppointmentDelayingService _appointmentDelayingService;

        public UrgentService(IDoctorOperationAvailabilityService doctorOperationAvailabilityService, IPatientOperationAvailabilityService patientOperationAvailabilityService, 
            IDoctorExaminationAvailabilityService doctorExaminationAvailabilityService, IPatientExaminationAvailabilityService patientExaminationAvailabilityService,
            ISchedulingService schedulingService, IOperationService operationService, IExaminationService examinationService, IPatientService patientService, 
            IDoctorService doctorService, IMedicalRecordService medicalRecordService, IAppointmentNotificationService appointmentNotificationService, 
            IAppointmentDelayingService appointmentDelayingService)
        {
            _doctorOperationAvailabilityService = doctorOperationAvailabilityService;
            _patientOperationAvailabilityService = patientOperationAvailabilityService;
            _doctorExaminationAvailabilityService = doctorExaminationAvailabilityService;
            _patientExaminationAvailabilityService = patientExaminationAvailabilityService;
            _schedulingService = schedulingService;
            _operationService = operationService;
            _examinationService = examinationService;
            _patientService = patientService;
            _doctorService = doctorService;
            _medicalRecordService = medicalRecordService;
            _appointmentNotificationService = appointmentNotificationService;
            _appointmentDelayingService = appointmentDelayingService;
        }

        public void SetExaminationDetails(Examination examination, ScheduleEditRequest selectedAppointment)
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
        public void SetOperationDetails(Operation operation, ScheduleEditRequest selectedAppointment)
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
        private List<DateTime> FindNextTwoHoursAppointments()
        {
            List<DateTime> possibleAppointments = new List<DateTime>();
            DateTime current = DateTime.Now;
            DateTime firstAppointment = current;

            if (current.Minute >= 0) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 15, 0);
            if (current.Minute >= 15) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 30, 0);
            if (current.Minute >= 30) firstAppointment = new DateTime(current.Year, current.Month, current.Day, current.Hour, 45, 0);
            if (current.Minute >= 45) firstAppointment = new DateTime(current.Year, current.Month, current.Day, (current.Hour + 1)%24, 0, 0);

            for (int i = 0; i <= 7; i++)
            {
                TimeSpan ts = new TimeSpan(0, 15, 0);
                possibleAppointments.Add(firstAppointment + i * ts);
            }
            return possibleAppointments;
        }
        private void TrySchedulingUrgentOperation(DateTime appointment, int duration, Doctor doctor, MedicalRecord medicalRecord, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
        {
            OperationDTO operationDTO = new OperationDTO(appointment, duration, null, doctor, medicalRecord);
            _doctorOperationAvailabilityService.CheckIfDoctorIsAvailable(operationDTO);
            _patientOperationAvailabilityService.CheckIfPatientIsAvailable(operationDTO);
            operationDTO.Room = _schedulingService.FindAvailableOperationRoom(operationDTO);
            Operation operation=_operationService.Add(operationDTO);
            _appointmentNotificationService.SendNotificationForNewOperation(operation);
            int id = _operationService.GetMaxId();
            priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(id, 2, appointment));
        }
        public List<Tuple<int, int, DateTime>> ReserveUrgentOperation(string patientUsername, SpecialtyType specialtyType, int duration)
        {
            List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
            Patient patient = _patientService.GetByUsername(patientUsername);
            var medicalRecord = _medicalRecordService.GetByPatientUsername(patient);
            List<DateTime> nextTwoHoursAppointments = FindNextTwoHoursAppointments();
            foreach (DateTime appointment in nextTwoHoursAppointments)
            {
                foreach (Doctor doctor in _doctorService.GetAll())
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
            int id = _operationService.GetMaxId();
            priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(id + 1, 2, new DateTime(1, 1, 1)));
            priorityExaminationsAndOperations.AddRange(_appointmentDelayingService.FindClosest(nextTwoHoursAppointments, specialtyType, Rooms.Model.RoomType.OperatingRoom));
            return priorityExaminationsAndOperations;
        }
        private void TrySchedulingUrgentExamination(DateTime appointment, Doctor doctor, MedicalRecord medicalRecord, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
        {
            ExaminationDTO examinationDTO = new ExaminationDTO(appointment, null, doctor, medicalRecord);
            _doctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
            _patientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
            examinationDTO.Room = _schedulingService.FindAvailableExaminationRoom(appointment);
            Examination examination=_examinationService.Add(examinationDTO);
            _appointmentNotificationService.SendNotificationForNewExamination(examination);
            int id = _examinationService.GetMaxId();
            priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(id, 2, appointment));
        }
        public List<Tuple<int, int, DateTime>> ReserveUrgentExamination(string patientUsername, SpecialtyType specialtyType)
        {
            List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
            Patient patient = _patientService.GetByUsername(patientUsername);
            var medicalRecord = _medicalRecordService.GetByPatientUsername(patient);
            List<DateTime> nextTwoHoursAppointments = FindNextTwoHoursAppointments();
            foreach (DateTime appointment in nextTwoHoursAppointments)
            {
                foreach (Doctor doctor in _doctorService.GetAll())
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
            int id = _examinationService.GetMaxId();
            priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(id + 1, 2, new DateTime(1, 1, 1)));
            priorityExaminationsAndOperations.AddRange(_appointmentDelayingService.FindClosest(nextTwoHoursAppointments, specialtyType, Rooms.Model.RoomType.ExaminationRoom));
            return priorityExaminationsAndOperations;
        }
    }
}
