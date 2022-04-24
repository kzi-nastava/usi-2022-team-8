using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations.Model;

public class RoomSeparation : Renovation
{
    public Room firstRoom { get; set; }
    public Room secondRoom { get; set; }

    public RoomSeparation(DateTime startDate, DateTime endDate, Room initialRoom, Room firstRoom, Room secondRoom) : base(startDate, endDate, initialRoom)
    {
        this.firstRoom = firstRoom;
        this.secondRoom = secondRoom;
    }
}