using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Renovations;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Rooms
{
    public class RoomService
    {
        private RoomRepository _roomRepository;
        private EquipmentTransferRepository _equipmentTransferRepository;
        private ExaminationRepository _examinationRepository;
        private OperationRepository _operationRepository;
        public RoomService()
        {
            _roomRepository = RoomRepository.GetInstance();
            _equipmentTransferRepository = EquipmentTransferRepository.GetInstance();
            _examinationRepository = ExaminationRepository.GetInstance();
            _operationRepository = OperationRepository.GetInstance();
        }

        public static bool CheckImportantOccurrenceOfRoom(Room room)
        {            
            if (EquipmentTransferService.CheckOccurrenceOfRoom(room))
            {
                return true;
            }
            
            if (SchedulingService.CheckOccurrenceOfRoom(room))
            {
                return true;
            }

            return false;
        }

        public static void MoveRoomToRenovationHistory(Room selectedRoom)
        {
            RoomRepository roomRepository = RoomRepository.GetInstance();
            if (RenovationService.CheckRenovationStatusForHistoryDelete(selectedRoom))
            {
                roomRepository.Delete(selectedRoom.Id);
            }
        }

        public static bool ExistsChangedRoomNumber(int number, Room oldRoom)
        {
            RoomRepository roomRepository = RoomRepository.GetInstance();
            int idx = roomRepository.FindIndexWithRoomNumber(number);
            if (idx >= 0)
            {
                if (roomRepository.GetAll()[idx] != oldRoom)
                {                    
                    return true;
                }
            }
            return false;
        }

        
    }
}
