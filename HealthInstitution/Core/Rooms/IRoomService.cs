using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Operations.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.GUI.ManagerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Rooms
{
    public interface IRoomService
    {
        public List<Room> GetAll();
        public Room AddRoom(RoomDTO roomDTO);
        public void Update(int id, RoomDTO roomDTO);
        public void Delete(int id);
        public void WriteIn();
        public bool CheckImportantOccurrenceOfRoom(Room room);
        public void MoveRoomToRenovationHistory(Room selectedRoom);
        public bool ExistsChangedRoomNumber(int number, Room oldRoom);
        public void AddToRoom(int id, Equipment equipment);
        public List<Room> GetActive();
        public List<Room> GetNotRenovating();
        public bool RoomNumberIsTaken(int number);
        public List<Equipment> GetAvailableEquipment(Room room);
        public List<TableItemEquipment> GetTableItemEquipments();
        public void UpdateEquipmentQuantity(Room room, Equipment equipment);
        public void RemoveEquipmentFrom(Room room);
    }
}
