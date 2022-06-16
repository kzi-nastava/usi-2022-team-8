using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations
{
    public interface IRenovationService
    {
        Renovation AddRenovation(RenovationDTO renovationDTO);
        Renovation AddRoomMerger(RoomMergerDTO roomMergerDTO);
        Renovation AddRoomSeparation(RoomSeparationDTO roomSeparationDTO);
        bool CheckRenovationStatusForHistoryDelete(Room room);
        bool CheckRenovationStatusForRoom(Room room, DateTime date);
        void Delete(int id);
        void End(Renovation renovation);
        List<Renovation> GetAll();
        void Start(Renovation renovation);
        void UpdateRenovation(int id, RenovationDTO renovationDTO);
        void UpdateRoomMerger(int id, RoomMergerDTO roomMergerDTO);
        void UpdateRoomSeparation(int id, RoomSeparationDTO roomSeparationDTO);
    }
}