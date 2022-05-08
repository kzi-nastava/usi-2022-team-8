using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations.Model;


public class RoomSeparation : Renovation
{
    public Room FirstRoom { get; set; }
    public Room SecondRoom { get; set; }

    public RoomSeparation(DateTime startDate, DateTime endDate, Room initialRoom, Room firstRoom, Room secondRoom) : base(startDate, endDate, initialRoom)
    {
        this.FirstRoom = firstRoom;
        this.SecondRoom = secondRoom;
    }
}