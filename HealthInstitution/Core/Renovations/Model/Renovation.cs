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

    public virtual bool HasActiveRooms()
    {
        return this.Room.IsActive;
    }

    public bool ShouldStart()
    {
        return this.StartDate <= DateTime.Today.AddDays(-1);
    }

    public bool ShouldEnd()
    {
        return this.EndDate <= DateTime.Today.AddDays(-1);
    }

    public virtual void Start()
    {
        this.Room.IsRenovating = true;
    }

    public virtual void End()
    {
        this.Room.IsRenovating = false;
    }

    public bool IsSimpleRenovation()
    {
        return this.GetType() == typeof(Renovation);
    }

    public bool IsRoomMerger()
    {
        return this.GetType() == typeof(RoomMerger);
    }

    public bool IsRoomSeparation()
    {
        return this.GetType() == typeof(RoomSeparation);
    }

    public virtual void RemoveOldRoomEquipment(){}

    public virtual bool CheckHistoryDelete(Room room)
    {
        if (this.Room == room)
        {
            room.IsActive = false;
            return true;
        }
        return false;
    }

    public virtual bool CheckRenovationStatus(Room room)
    {
        if (this.Room == room)
        {
            return true;
        }
        return false;
    }
}