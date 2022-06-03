using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Rooms.Repository
{
    public interface IRoomRepository : IRepository<Room>
    {
        public List<Equipment> ConvertJTokenToEquipments(JToken tokens);
        public Room Parse(JToken? room);
        public void LoadFromFile();
        public List<int> FormListOfIds(List<Equipment> equipments);
        public List<dynamic> PrepareForSerialization();
        public void Save();
        public List<Room> GetAll();
        public Room GetById(int id);
        public Room AddRoom(Room room);
        public void Update(int id, Room byRoom);
        public void Delete(int id);
        public void AddToRoom(int id, Equipment equipment);
        public List<Room> GetActive();
        public List<Room> GetNotRenovating();
        public List<Equipment> GetDynamicEquipment(Room room);
        public Room? GetRoomFromString(string? roomFromForm);
        public bool RoomNumberIsTaken(int number);
        public int FindIndexWithRoomNumber(int number);
    }
}
