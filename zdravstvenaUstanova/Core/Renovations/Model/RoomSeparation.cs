using zdravstvenaUstanova.Core.Rooms.Model;

namespace zdravstvenaUstanova.Core.Renovations.Model;

public class RoomSeparation : Room
{
    public Room firstRoom { get; set; }
    public Room secondRoom { get; set; }

    public RoomSeparation(Room firstRoom, Room secondRoom)
    {
        this.firstRoom = firstRoom;
        this.secondRoom = secondRoom;
    }
}