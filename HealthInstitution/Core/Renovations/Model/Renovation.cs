using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations.Model;

public class Renovation
{
    public int Id { get; set; }
    public Room Room { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    

    public Renovation(int id, Room room, DateTime startDate, DateTime endDate)
    {
        this.Id = id;
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.Room = room;
    }

    public bool IsRoomMerger()
    {
        return this.GetType() == typeof(RoomMerger);
    }
}