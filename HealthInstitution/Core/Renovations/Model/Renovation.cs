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

    public Renovation(RenovationDTO renovationDTO)
    {
        this.Room = renovationDTO.Room;
        this.StartDate = renovationDTO.StartDate;
        this.EndDate = renovationDTO.EndDate;
    }

    public bool IsRoomMerger()
    {
        return this.GetType() == typeof(RoomMerger);
    }

    public bool IsRoomSeparation()
    {
        return this.GetType() == typeof(RoomSeparation);
    }
}