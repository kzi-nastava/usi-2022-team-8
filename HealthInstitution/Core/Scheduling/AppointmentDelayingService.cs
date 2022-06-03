using HealthInstitution.Core.Examinations;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Operations;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Rooms;
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
        public static DateTime FindFirstAvailableAppointment(DateTime appointment, int appointmentCounter, TimeSpan ts)
        {
            DateTime firstAvailableAppointment = appointment + appointmentCounter * ts;
            if (firstAvailableAppointment.Hour > 22)
            {
                firstAvailableAppointment += new TimeSpan(9, 0, 0);
            }
            return firstAvailableAppointment;
        }

        public static void GetExaminationsWithPriorities(List<Examination> nextTwoHoursExaminations, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
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
                    if (RoomService.FindAllAvailableRooms(firstAvailableAppointment).Contains(examination.Room))
                    {
                        priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(examination.Id, 1, firstAvailableAppointment));
                        break;
                    }
                }
            }
        }

        public static void GetOperationsWithPriorities(List<Operation> nextTwoHoursOperations, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations)
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

                    if (RoomService.FindAllAvailableRooms(firstAvailableAppointment).Contains(operation.Room))
                    {
                        priorityExaminationsAndOperations.Add(new Tuple<int, int, DateTime>(operation.Id, 0, firstAvailableAppointment));
                        break;
                    }
                }
            }
        }
        public static List<Tuple<int, int, DateTime>> GetPriorityExaminationsAndOperations(List<Examination> nextTwoHoursExaminations, List<Operation> nextTwoHoursOperations)
        {
            List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations = new List<Tuple<int, int, DateTime>>();
            GetExaminationsWithPriorities(nextTwoHoursExaminations, priorityExaminationsAndOperations);
            GetOperationsWithPriorities(nextTwoHoursOperations, priorityExaminationsAndOperations);
            priorityExaminationsAndOperations.Sort((x, y) => y.Item3.CompareTo(x.Item3));
            return priorityExaminationsAndOperations;
        }

        public static List<Tuple<int, int, DateTime>> FindClosest(List<DateTime> nextTwoHoursAppointments, SpecialtyType specialtyType)
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
            return GetPriorityExaminationsAndOperations(nextTwoHoursExaminations, nextTwoHoursOperations);
        }

        public static List<DateTime> FindNextTwoHoursAppointments()
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
    }
}
