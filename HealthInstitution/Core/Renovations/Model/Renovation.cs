using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations.Model;

public class Renovation
{
    public DateTime startDate { get; set; }

    public DateTime endDate { get; set; }
    public Room room { get; set; }

    public Renovation(DateTime startDate, DateTime endDate, Room room)
    {
        this.startDate = startDate;
        this.endDate = endDate;
        this.room = room;
    }
}