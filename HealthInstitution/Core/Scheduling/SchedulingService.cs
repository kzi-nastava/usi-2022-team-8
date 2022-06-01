using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Scheduling
{
    public static class SchedulingService
    {
        //koristiti urgentService

        private static ExaminationRepository s_examinationRepository = ExaminationRepository.GetInstance();
        private static OperationRepository s_operationRepository = OperationRepository.GetInstance();

        public static bool CheckOccurrenceOfRoom(Room room)
        {
            if (s_examinationRepository.Examinations.Find(examination => examination.Room == room) == null)
            {
                return false;
            }

            if (s_operationRepository.Operations.Find(operation => operation.Room == room) == null)
            {
                return false;
            }
            return true;
        }
    }
}
