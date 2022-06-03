using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Notifications;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    internal static class AppointmentDelayingService
    {
        private static DateTime FindFirstAvailableAppointment(DateTime appointment, int appointmentCounter, TimeSpan ts)
        {
            DateTime firstAvailableAppointment = appointment + appointmentCounter * ts;
            if (firstAvailableAppointment.Hour > 22)
            {
                firstAvailableAppointment += new TimeSpan(9, 0, 0);
            }
            return firstAvailableAppointment;
        }
        private static void GetExaminationsWithPriorities(List<Examination> nextTwoHoursExaminations, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations, RoomType roomType)
        {
            int appointmentCounter;
            DateTime firstAvailableAppointment;
            foreach (Examination examination in nextTwoHoursExaminations)
            {
                appointmentCounter = 1;
                while (true)
                {
                    firstAvailableAppointment = FindFirstAvailableAppointment(examination.Appointment, appointmentCounter, new TimeSpan(0, 15, 0));
                    appointmentCounter++;
                    try
                    {
                        ExaminationDTO examinationDTO = new ExaminationDTO(firstAvailableAppointment, null, examination.Doctor, examination.MedicalRecord);
                        DoctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
                        PatientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
                    }
                    catch
                    {
                        continue;
                    }
                    if (SchedulingService.FindAllAvailableRooms(roomType,firstAvailableAppointment).Contains(examination.Room))
                    {
                        priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(examination.Id, 1, firstAvailableAppointment));
                        break;
                    }
                }
            }
        }
        private static void GetOperationsWithPriorities(List<Operation> nextTwoHoursOperations, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations, RoomType roomType)
        {
            int appointmentCounter;
            DateTime firstAvailableAppointment;
            foreach (Operation operation in nextTwoHoursOperations)
            {
                appointmentCounter = 1;
                while (true)
                {
                    firstAvailableAppointment = FindFirstAvailableAppointment(operation.Appointment, appointmentCounter, new TimeSpan(0, operation.Duration, 0));
                    appointmentCounter++;
                    try
                    {
                        ExaminationDTO examinationDTO = new ExaminationDTO(firstAvailableAppointment, null, operation.Doctor, operation.MedicalRecord);
                        DoctorExaminationAvailabilityService.CheckIfDoctorIsAvailable(examinationDTO);
                        PatientExaminationAvailabilityService.CheckIfPatientIsAvailable(examinationDTO);
                    }
                    catch
                    {
                        continue;
                    }

                    if (SchedulingService.FindAllAvailableRooms(roomType, firstAvailableAppointment).Contains(operation.Room))
                    {
                        priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(operation.Id, 0, firstAvailableAppointment));
                        break;
                    }
                }
            }
        }
        private static List<Tuple<int, int, DateTime>> GetPriorityExaminationsAndOperations(List<Examination> nextTwoHoursExaminations, List<Operation> nextTwoHoursOperations, RoomType roomType)
        {
            List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
            GetExaminationsWithPriorities(nextTwoHoursExaminations, priorityExaminationsAndOperations, roomType);
            GetOperationsWithPriorities(nextTwoHoursOperations, priorityExaminationsAndOperations, roomType);
            priorityExaminationsAndOperations.Sort((x, y) => y.Item3.CompareTo(x.Item3));
            return priorityExaminationsAndOperations;
        }
        public static List<Tuple<int, int, DateTime>> FindClosest(List<DateTime> nextTwoHoursAppointments, SpecialtyType specialtyType, RoomType roomType)
        {
            List<Examination> nextTwoHoursExaminations = new List<Examination>();
            List<Operation> nextTwoHoursOperations = new List<Operation>();
            foreach (Examination examination in ExaminationService.GetAll())
            {
                if (nextTwoHoursAppointments.Contains(examination.Appointment) && examination.Doctor.Specialty == specialtyType)
                    nextTwoHoursExaminations.Add(examination);
            }
            foreach (Operation operation in OperationService.GetAll())
            {
                if (nextTwoHoursAppointments.Contains(operation.Appointment) && operation.Doctor.Specialty == specialtyType)
                    nextTwoHoursOperations.Add(operation);
            }
            return GetPriorityExaminationsAndOperations(nextTwoHoursExaminations, nextTwoHoursOperations, roomType);
        }
        public static List<ScheduleEditRequest> PrepareDataForDelaying(List<Tuple<int, int, DateTime>> examinationsAndOperationsForDelaying)
        {
            List<ScheduleEditRequest> delayedAppointments = new List<ScheduleEditRequest>();
            foreach (Tuple<int, int, DateTime> tuple in examinationsAndOperationsForDelaying)
            {
                if (tuple.Item2 == 1)
                {
                    Examination currentExamination = ExaminationService.GetById(tuple.Item1);
                    Examination newExamination = new Examination(currentExamination.Id, ExaminationStatus.Scheduled, tuple.Item3, currentExamination.Room, currentExamination.Doctor, currentExamination.MedicalRecord, "");
                    delayedAppointments.Add(new ScheduleEditRequest(0, currentExamination, newExamination, Core.RestRequests.Model.RestRequestState.OnHold));
                }
                if (tuple.Item2 == 0)
                {
                    Operation currentOperation = OperationService.GetById(tuple.Item1);
                    Operation newOperation = new Operation(currentOperation.Id, tuple.Item3, currentOperation.Duration, currentOperation.Room, currentOperation.Doctor, currentOperation.MedicalRecord);
                    delayedAppointments.Add(new ScheduleEditRequest(0, currentOperation, newOperation, Core.RestRequests.Model.RestRequestState.OnHold));
                }
            }
            return delayedAppointments;
        }
        public static void DelayExamination(ScheduleEditRequest selectedAppointment, Examination examination)
        {
            ExaminationRepository.GetInstance().SwapExaminationValue(selectedAppointment.NewExamination);
            UrgentService.SetExaminationDetails(examination, selectedAppointment);
            ExaminationDTO examinationDTO = new ExaminationDTO(examination.Appointment, examination.Room, examination.Doctor, examination.MedicalRecord);
            ExaminationService.Add(examinationDTO);
            AppointmentNotificationService.SendNotificationsForDelayedExamination(selectedAppointment);
            AppointmentNotificationService.SendNotificationForNewExamination(examination);
        }
        public static void DelayOperation(ScheduleEditRequest selectedAppointment, Operation operation)
        {
            OperationRepository.GetInstance().SwapOperationValue(selectedAppointment.NewOperation);
            UrgentService.SetOperationDetails(operation, selectedAppointment);
            OperationDTO operationDTO = new OperationDTO(operation.Appointment, operation.Duration, operation.Room, operation.Doctor, operation.MedicalRecord);
            OperationService.Add(operationDTO);
            AppointmentNotificationService.SendNotificationsForDelayedOperation(selectedAppointment);
            AppointmentNotificationService.SendNotificationForNewOperation(operation);
        }
    }
}
