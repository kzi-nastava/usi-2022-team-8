using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Renovations;
using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Rooms
{
    public class RoomTimetableService : IRoomTimetableService
    {
        IOperationRepository _operationRepository;
        IExaminationRepository _examinationRepository;
        IEquipmentTransferService _equipmentTransferService;
        IRenovationService _renovationService;

        public RoomTimetableService(IOperationRepository operationRepository, IExaminationRepository examinationRepository,
            IEquipmentTransferService equipmentTransferService, IRenovationService renovationService)
        {
            _operationRepository = operationRepository;
            _examinationRepository = examinationRepository;
            _equipmentTransferService = equipmentTransferService;
            _renovationService = renovationService;
        }
        public bool CheckRoomTimetable(Room selectedRoom, DateTime startDate, out string message)
        {
            if (CheckIfRoomHasScheduledExamination(selectedRoom))
            {
                message = "Room has scheduled examination!";
                return true;
            }

            if (CheckIfRoomHasScheduledOperation(selectedRoom))
            {
                message = "Room has scheduled operation!";
                return true;
            }

            if (CheckIfRoomHasScheduledRenovation(selectedRoom))
            {
                message = "Room is already scheduled for renovation!";
                return true;
            }

            if (CheckIfRoomHasScheduledEquipmentTransfer(selectedRoom, startDate))
            {
                message = "Room has equipment transfer in that date span!";
                return true;
            }
            message = "";
            return false;
        }

        private bool CheckIfRoomHasScheduledEquipmentTransfer(Room selectedRoom, DateTime startDate)
        {
            foreach (EquipmentTransfer equipmentTransfer in _equipmentTransferService.GetAll())
            {
                if (equipmentTransfer.TransferTime < startDate)
                {
                    continue;
                }
                if (equipmentTransfer.FromRoom == selectedRoom || equipmentTransfer.ToRoom == selectedRoom)
                {
                    return true;
                }

            }
            return false;
        }

        private bool CheckIfRoomHasScheduledRenovation(Room selectedRoom)
        {
            foreach (Renovation renovation in _renovationService.GetAll())
            {
                if (renovation.Room == selectedRoom)
                {
                    return true;
                }
                if (renovation.IsRoomMerger() && ((RoomMerger)renovation).RoomForMerge == selectedRoom)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckIfRoomHasScheduledOperation(Room selectedRoom)
        {
            foreach (Operation operation in _operationRepository.GetAll())
            {
                if (operation.Room == selectedRoom)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckIfRoomHasScheduledExamination(Room selectedRoom)
        {
            foreach (Examination examination in _examinationRepository.GetAll())
            {
                if (examination.Room == selectedRoom)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
