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
    internal class SchedulingService
    {
        //koristiti urgentService

        private ExaminationRepository _examinationRepository;
        private OperationRepository _operationRepository;

        public SchedulingService()
        {
            _examinationRepository = ExaminationRepository.GetInstance();
            _operationRepository = OperationRepository.GetInstance();
        }

        public static bool CheckOccurrenceOfRoom(Room room)
        {
            ExaminationRepository examinationRepository = ExaminationRepository.GetInstance();
            if (examinationRepository.Examinations.Find(examination => examination.Room == room) == null)
            {
                return false;
            }

            OperationRepository operationRepository = OperationRepository.GetInstance();
            if (operationRepository.Operations.Find(operation => operation.Room == room) == null)
            {
                return false;
            }
            return true;
        }
    }
}
