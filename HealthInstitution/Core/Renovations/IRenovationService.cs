using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Renovations
{
    public interface IRenovationService
    {
        public List<Renovation> GetAll();
        public void AddRenovation(RenovationDTO renovationDTO);
        public void AddRoomMerger(RoomMergerDTO roomMergerDTO);
        public void AddRoomSeparation(RoomSeparationDTO roomSeparationDTO);
        public void UpdateRenovation(int id, RenovationDTO renovationDTO);
        public void UpdateRoomMerger(int id, RoomMergerDTO roomMergerDTO);
        public void UpdateRoomSeparation(int id, RoomSeparationDTO roomSeparationDTO);
        public void Delete(int id);
        public void StartRenovation(Room room);
        public void EndRenovation(Room room);
        public void StartMerge(Room firstRoom, Room secondRoom, Room mergedRoom);
        public void EndMerge(Room firstRoom, Room secondRoom, Room mergedRoom);
        public void StartSeparation(Room separationRoom, Room firstRoom, Room secondRoom);
        public void EndSeparation(Room separationRoom, Room firstRoom, Room secondRoom);
        public bool CheckRenovationStatusForHistoryDelete(Room room);
        public bool CheckRenovationStatusForRoom(Room room, DateTime date);
    }
}
