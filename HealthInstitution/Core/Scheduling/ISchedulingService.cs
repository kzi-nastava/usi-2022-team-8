using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.MedicalRecords.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Referrals.Model;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public interface ISchedulingService
    {
        public bool CheckOccurrenceOfRoom(Room room);
        public void RedirectByType(Referral referral, DateTime appointment, MedicalRecord medicalRecord);
        public void ScheduleWithSpecificDoctor(DateTime appointment, Referral referral, MedicalRecord medicalRecord);
        public void ScheduleWithOrderedSpecialist(ExaminationDTO examination, Referral referral);
        public void ScheduleWithOrderedSpecialty(DateTime appointment, Referral referral, MedicalRecord medicalRecord);
        public void ReserveOperation(OperationDTO operationDTO, int id = 0);
        public void ReserveExamination(ExaminationDTO examinationDTO);
        public Room FindAvailableOperationRoom(OperationDTO operationDTO, int id = 0);
        public Room FindAvailableExaminationRoom(DateTime appointment);
        public List<Room> FindAllAvailableRooms(RoomType roomType, DateTime appointment);
    }
}
