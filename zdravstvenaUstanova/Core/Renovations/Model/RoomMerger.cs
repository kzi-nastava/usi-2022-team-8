using zdravstvenaUstanova.Core.Rooms.Model;

namespace zdravstvenaUstanova.Core.Renovations.Model;

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