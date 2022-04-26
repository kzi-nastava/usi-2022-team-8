using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Rooms.Repository
{
    public class RoomRepository
    {
        public String fileName { get; set; }
        public List<Room> rooms { get; set; }
        public Dictionary<int, Room> roomById { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private RoomRepository(String fileName)
        {
            this.fileName = fileName;
            this.rooms = new List<Room>();
            this.roomById = new Dictionary<int, Room>();
            this.LoadRooms();
        }
        private static RoomRepository instance = null;
        public static RoomRepository GetInstance()
        {
            {
                if (instance == null)
                {
                    instance = new RoomRepository(@"..\..\..\Data\JSON\rooms.json");
                }
                return instance;
            }
        }
        public void LoadRooms()
        {
            var rooms = JsonSerializer.Deserialize<List<Room>>(File.ReadAllText(@"..\..\..\Data\JSON\rooms.json"), options);
            foreach (Room room in rooms)
            {
                this.rooms.Add(room);
                this.roomById.Add(room.id, room);
            }
        }

        public void SaveRooms()
        {
            var allRooms = JsonSerializer.Serialize(this.rooms, options);
            File.WriteAllText(this.fileName, allRooms);
        }

        public List<Room> GetRooms()
        {
            return this.rooms;
        }

        public void AddRoom(int id, RoomType type, int number, bool isRenovating)
        {
            Room room = new Room(id, type, number, isRenovating);
            this.rooms.Add(room);
            this.roomById.Add(room.id, room);
            SaveRooms();
        }

        public void UpdateRoom(int id, RoomType type, int number, bool isRenovating)
        {
            Room room = roomById[id];
            room.type = type;
            room.number = number;
            room.isRenovating = isRenovating;
            SaveRooms();
        }


        public void DeleteRoom(int id)
        {
            Room room = roomById[id];
            this.rooms.Remove(room);
            this.roomById.Remove(id);
            SaveRooms();
        }
    }
}
