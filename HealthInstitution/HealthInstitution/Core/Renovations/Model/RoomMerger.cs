using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations.Model;

public class RoomMerger : Room
{
    public Room roomForMerge { get; set; }
    public Room mergedRoom { get; set; }

    public RoomMerger(Room roomForMerge, Room mergedRoom)
    {
        this.roomForMerge = roomForMerge;
        this.mergedRoom = mergedRoom;
    }
    
}