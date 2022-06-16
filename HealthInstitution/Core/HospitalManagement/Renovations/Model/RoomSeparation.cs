using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
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

    public RoomSeparation(RoomSeparationDTO roomSeparationDTO) : base(roomSeparationDTO)
    {
        this.FirstRoom = roomSeparationDTO.FirstRoom;
        this.SecondRoom = roomSeparationDTO.SecondRoom;
    }

    public override bool HasActiveRooms()
    {
        return base.HasActiveRooms();
    }

    public override void Start()
    {
        this.Room.IsRenovating = true;
    }

    public override void End()
    { 
        this.Room.ExcludeByRenovation();
        this.FirstRoom.ActivateByRenovation();
        this.SecondRoom.ActivateByRenovation();
    }

    public override void RemoveOldRoomEquipment()
    {
        Room.AvailableEquipment.Clear();
    }

    public override bool CheckHistoryDelete(Room room)
    {
        return base.CheckHistoryDelete(room);
    }

    public override bool CheckRenovationStatus(Room room)
    {
        return base.CheckRenovationStatus(room);
    }
}