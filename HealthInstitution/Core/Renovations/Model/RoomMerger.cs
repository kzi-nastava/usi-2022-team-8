using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations.Model;

public class RoomMerger : Renovation
{
    public Room RoomForMerge { get; set; }
    public Room MergedRoom { get; set; }
    public RoomMerger(int id, Room initialRoom, Room roomForMerge, Room mergedRoom, DateTime startDate, DateTime endDate): base(id, initialRoom,startDate, endDate)
    {
        this.RoomForMerge = roomForMerge;
        this.MergedRoom = mergedRoom;
    }

    public RoomMerger(RoomMergerDTO roomMergerDTO) : base(roomMergerDTO)
    {
        this.RoomForMerge = roomMergerDTO.RoomForMerge;
        this.MergedRoom = roomMergerDTO.MergedRoom;
    }
    
}