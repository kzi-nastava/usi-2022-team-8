using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.EquipmentTransfers;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Operations.Repository;
using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Renovations.Repository;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Renovations
{
    public static class RenovationService
    {
        private static RenovationRepository s_renovationRepository = RenovationRepository.GetInstance();
        private static OperationRepository s_operationRepository = OperationRepository.GetInstance();
        private static ExaminationRepository s_examinationRepository = ExaminationRepository.GetInstance();
        public static List<Renovation> GetAll()
        {
            return s_renovationRepository.GetAll();
        }

        public static Renovation AddRenovation(RenovationDTO renovationDTO)
        {
            Renovation renovation = new Renovation(renovationDTO);
            s_renovationRepository.AddRenovation(renovation);
            return renovation;
        }
        public static Renovation AddRoomMerger(RoomMergerDTO roomMergerDTO)
        {
            Renovation renovation = new RoomMerger(roomMergerDTO);
            s_renovationRepository.AddRenovation(renovation);
            return renovation;
        }
        
        public static Renovation AddRoomSeparation(RoomSeparationDTO roomSeparationDTO)
        {
            Renovation renovation = new RoomSeparation(roomSeparationDTO);
            s_renovationRepository.AddRenovation(renovation);
            return renovation;
        }

        public static void UpdateRenovation(int id, RenovationDTO renovationDTO)
        {
            Renovation renovation = new Renovation(renovationDTO);
            s_renovationRepository.UpdateRenovation(id, renovation);
        }
        public static void UpdateRoomMerger(int id, RoomMergerDTO roomMergerDTO)
        {
            RoomMerger roomMerger = new RoomMerger(roomMergerDTO);
            s_renovationRepository.UpdateRoomMerger(id, roomMerger);
        }
        public static void UpdateRoomSeparation(int id, RoomSeparationDTO roomSeparationDTO)
        {
            RoomSeparation roomSeparation = new RoomSeparation(roomSeparationDTO);
            s_renovationRepository.UpdateRoomSeparation(id, roomSeparation);
        }
        public static void Delete(int id)
        {
            s_renovationRepository.Delete(id);
        }

        public static void Start(Renovation renovation)
        {
            renovation.Start();
            RoomService.WriteIn();
        }

        public static void End(Renovation renovation)
        {
            renovation.End();
            renovation.RemoveOldRoomEquipment();
            RoomService.WriteIn();
        }

        public static bool CheckRenovationStatusForHistoryDelete(Room room)
        {
            foreach (Renovation renovation in s_renovationRepository.Renovations)
            {
                if (renovation.CheckHistoryDelete(room))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CheckRenovationStatusForRoom(Room room, DateTime date)
        {
            foreach (Renovation renovation in s_renovationRepository.Renovations)
            {
                if (CheckForProjectedSeparation(renovation,date))
                {
                    continue;
                }
                if (renovation.CheckRenovationStatus(room))       
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CheckForProjectedSeparation(Renovation renovation, DateTime date)
        {
            return renovation.StartDate > date && renovation.IsRoomSeparation();
        }

    }


}
