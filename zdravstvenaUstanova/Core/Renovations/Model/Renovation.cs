using zdravstvenaUstanova.Core.Rooms.Model;

namespace zdravstvenaUstanova.Core.Renovations.Model;

public class Renovation
{
    public DateTime startDate { get; set; }
    public DateTime endTime { get; set; }
    public Room room { get; set; }

    public Renovation(DateTime startDate, DateTime endTime, Room room)
    {
        this.startDate = startDate;
        this.endTime = endTime;
        this.room = room;
    }
}