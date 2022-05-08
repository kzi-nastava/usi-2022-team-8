using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations.Model;

public class Renovation
{
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    public Room Room { get; set; }

    public Renovation(DateTime startDate, DateTime endDate, Room room)
    {
        this.StartDate = startDate;
        this.EndDate = endDate;
        this.Room = room;
    }
}