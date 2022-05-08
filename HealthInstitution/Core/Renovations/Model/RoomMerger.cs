using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations.Model;

public class RoomMerger : Renovation
{
    public Room RoomForMerge { get; set; }
    public Room MergedRoom { get; set; }
    public RoomMerger(DateTime startDate, DateTime endDate, Room initialRoom, Room roomForMerge, Room mergedRoom): base(startDate, endDate, initialRoom)
    {
        this.RoomForMerge = roomForMerge;
        this.MergedRoom = mergedRoom;
    }
    
}