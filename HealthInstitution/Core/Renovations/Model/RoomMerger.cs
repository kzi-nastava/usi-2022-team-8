using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations.Model;

public class RoomMerger : Renovation
{
    public Room roomForMerge { get; set; }
    public Room mergedRoom { get; set; }
    public RoomMerger(DateTime startDate, DateTime endDate, Room initialRoom, Room roomForMerge, Room mergedRoom): base(startDate, endDate, initialRoom)
    {
        this.roomForMerge = roomForMerge;
        this.mergedRoom = mergedRoom;
    }
    
}