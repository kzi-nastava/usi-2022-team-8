using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.ScheduleEditRequests.Model;
using HealthInstitution.Core.SystemUsers.Doctors.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public interface IUrgentService
    {
        public List<ScheduleEditRequest> PrepareDataForDelaying(List<Tuple<int, int, DateTime>> examinationsAndOperationsForDelaying);
        public void DelayExamination(ScheduleEditRequest selectedAppointment, Examination examination);
        public void DelayOperation(ScheduleEditRequest selectedAppointment, Operation operation);
        public void SendNotificationsForExamination(Examination examination, ScheduleEditRequest selectedAppointment);
        public void SendNotificationsForOperation(Operation operation, ScheduleEditRequest selectedAppointment);
        public void SetExaminationDetails(Examination examination, ScheduleEditRequest selectedAppointment);
        public void SetOperationDetails(Operation operation, ScheduleEditRequest selectedAppointment);
        public void TrySchedulingUrgentOperation(DateTime appointment, int duration, Doctor doctor, MedicalRecord medicalRecord, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations);
        public List<Tuple<int, int, DateTime>> ReserveUrgentOperation(string patientUsername, SpecialtyType specialtyType, int duration);
        public List<Tuple<int, int, DateTime>> ReserveUrgentExamination(string patientUsername, SpecialtyType specialtyType);
        public void TrySchedulingUrgentExamination(DateTime appointment, Doctor doctor, MedicalRecord medicalRecord, List<Tuple<int, int, DateTime>> priorityExaminationsAndOperations);
    }
}
