using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations.Model;


public class RoomSeparation : Renovation
{
    public Room FirstRoom { get; set; }
    public Room SecondRoom { get; set; }

    public RoomSeparation(int id, Room initialRoom, Room firstRoom, Room secondRoom, DateTime startDate, DateTime endDate) : base(id, initialRoom, startDate, endDate)
    {
        this.FirstRoom = firstRoom;
        this.SecondRoom = secondRoom;
    }
}