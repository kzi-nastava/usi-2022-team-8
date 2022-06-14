using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms.Model;

namespace HealthInstitution.Core.Renovations.Model;

public class RoomMerger : Renovation
{
    public Room RoomForMerge { get; set; }
    public Room MergedRoom { get; set; }
    public RoomMerger(int id, Room initialRoom, Room roomForMerge, Room mergedRoom, DateTime startDate, DateTime endDate): base(id, initialRoom,startDate, endDate)
    {
        this.RoomForMerge = roomForMerge;
        this.MergedRoom = mergedRoom;
    }

    public RoomMerger(RoomMergerDTO roomMergerDTO) : base(roomMergerDTO)
    {
        this.RoomForMerge = roomMergerDTO.RoomForMerge;
        this.MergedRoom = roomMergerDTO.MergedRoom;
    }

    public override bool HasActiveRooms()
    {
        return this.Room.IsActive && this.MergedRoom.IsActive;
    }

    public override void Start()
    {
        this.Room.IsRenovating = true;
        this.RoomForMerge.IsRenovating = true;
    }

    public override void End()
    {
        foreach (Equipment equipment in this.Room.AvailableEquipment)
        {
            this.MergedRoom.AvailableEquipment.Add(equipment);
        }
        this.Room.AvailableEquipment.Clear();

        foreach (Equipment equipment in this.RoomForMerge.AvailableEquipment)
        {
            UpdateEquipmentQuantity(equipment);
        }
        this.RoomForMerge.AvailableEquipment.Clear();

        this.Room.IsRenovating = false;
        this.Room.IsActive = false;
        this.RoomForMerge.IsRenovating = false;
    }

    private void UpdateEquipmentQuantity(Equipment equipment)
    {
        int index = MergedRoom.AvailableEquipment.FindIndex(eq => eq.Name == equipment.Name && eq.Type == equipment.Type);
        if (index >= 0)
        {
            MergedRoom.AvailableEquipment[index].Quantity += equipment.Quantity;
        }
        else
        {
            MergedRoom.AvailableEquipment.Add(equipment);
        }
    }
}