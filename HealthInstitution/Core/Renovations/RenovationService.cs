using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Renovations.Repository;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations
{
    public class RenovationService : IRenovationService
    {
        IRenovationRepository _renovationRepository;
        IRoomService _roomService;
        public RenovationService(IRenovationRepository renovationRepository,
            IRoomService roomService)
        {
            _renovationRepository = renovationRepository;
            _roomService = roomService;
        }
        public List<Renovation> GetAll()
        {
            return _renovationRepository.GetAll();
        }

        public Renovation AddRenovation(RenovationDTO renovationDTO)
        {
            Renovation renovation = new Renovation(renovationDTO);
            _renovationRepository.AddRenovation(renovation);
            return renovation;
        }
        public Renovation AddRoomMerger(RoomMergerDTO roomMergerDTO)
        {
            Renovation renovation = new RoomMerger(roomMergerDTO);
            _renovationRepository.AddRenovation(renovation);
            return renovation;
        }

        public Renovation AddRoomSeparation(RoomSeparationDTO roomSeparationDTO)
        {
            Renovation renovation = new RoomSeparation(roomSeparationDTO);
            _renovationRepository.AddRenovation(renovation);
            return renovation;
        }

        public void UpdateRenovation(int id, RenovationDTO renovationDTO)
        {
            Renovation renovation = new Renovation(renovationDTO);
            _renovationRepository.UpdateRenovation(id, renovation);
        }
        public void UpdateRoomMerger(int id, RoomMergerDTO roomMergerDTO)
        {
            RoomMerger roomMerger = new RoomMerger(roomMergerDTO);
            _renovationRepository.UpdateRoomMerger(id, roomMerger);
        }
        public void UpdateRoomSeparation(int id, RoomSeparationDTO roomSeparationDTO)
        {
            RoomSeparation roomSeparation = new RoomSeparation(roomSeparationDTO);
            _renovationRepository.UpdateRoomSeparation(id, roomSeparation);
        }
        public void Delete(int id)
        {
            _renovationRepository.Delete(id);
        }

        public void Start(Renovation renovation)
        {
            renovation.Start();
            _roomService.WriteIn();
        }

        public void End(Renovation renovation)
        {
            renovation.End();
            renovation.RemoveOldRoomEquipment();
            _roomService.WriteIn();
        }

        public bool CheckRenovationStatusForHistoryDelete(Room room)
        {
            foreach (Renovation renovation in _renovationRepository.GetAll())
            {
                if (renovation.CheckHistoryDelete(room))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckRenovationStatusForRoom(Room room, DateTime date)
        {
            foreach (Renovation renovation in _renovationRepository.GetAll())
            {
                if (CheckForProjectedSeparation(renovation, date))
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

        private bool CheckForProjectedSeparation(Renovation renovation, DateTime date)
        {
            return renovation.StartDate > date && renovation.IsRoomSeparation();
        }

    }


}
