using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.Rooms.Model;
using Newtonsoft.Json.Linq;
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
        private String _fileName;

        private int _maxId;
        public List<Room> Rooms { get; set; }
        public Dictionary<int, Room> RoomById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
        private RoomRepository(String fileName)
        {
            this._fileName = fileName;
            this.Rooms = new List<Room>();
            this.RoomById = new Dictionary<int, Room>();
            this._maxId = 0;
            this.LoadFromFile();
        }
        private static RoomRepository s_instance = null;
        public static RoomRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new RoomRepository(@"..\..\..\Data\JSON\rooms.json");
                }
                return s_instance;
            }
        }

        private List<Equipment> ConvertJTokenToEquipments(JToken tokens)
        {
            var equipmentById = EquipmentRepository.GetInstance().EquipmentById;
            List<Equipment> equipments = new List<Equipment>();
            foreach (JToken equipmentToken in tokens)
            {
                Equipment equipment = equipmentById[(int)equipmentToken];
                equipments.Add(equipment);
            }
            return equipments;
        }

        private Room Parse(JToken? room)
        {
            int id = (int)room["id"];
            RoomType type;
            Enum.TryParse(room["type"].ToString(), out type);
            int number = (int)room["number"];
            bool isRenovating = (bool)room["isRenovating"];
            bool isActive = (bool)room["isActive"];
            List<Equipment> availableEquipment = ConvertJTokenToEquipments(room["availableEquipment"]);

            return new Room(id, type, number, isRenovating, availableEquipment, isActive);
        }
        public void LoadFromFile()
        {
            var rooms = JArray.Parse(File.ReadAllText(_fileName));
            foreach (var room in rooms)
            {
                Room loadedRoom = Parse(room);
                int id = loadedRoom.Id;
                if (id > _maxId)
                {
                    _maxId = id;
                }

                this.Rooms.Add(loadedRoom);
                this.RoomById.Add(id, loadedRoom);
            }
        }

        private List<int> FormListOfIds(List<Equipment> equipments)
        {
            var ids = new List<int>();
            foreach(var equipment in equipments)
            {
                ids.Add(equipment.Id);
            }
            return ids;
        }
        private List<dynamic> PrepareForSerialization()
        {
            List<dynamic> reducedRooms = new List<dynamic>();
            foreach (var room in this.Rooms)
            {
                reducedRooms.Add(new
                {
                    id = room.Id,
                    type = room.Type,
                    number = room.Number,
                    isRenovating = room.IsRenovating,
                    isActive = room.IsActive,
                    availableEquipment = FormListOfIds(room.AvailableEquipment)
                });
            }
            return reducedRooms;
        }
        public void Save()
        {
            var allRooms = JsonSerializer.Serialize(PrepareForSerialization(), _options);
            File.WriteAllText(this._fileName, allRooms);
        }

        public List<Room> GetAll()
        {
            return this.Rooms;
        }

        public Room GetById(int id)
        {
            if (RoomById.ContainsKey(id))
                return RoomById[id];
            return null;
        }

        public Room AddRoom(RoomDTO roomDTO)
        {

            this._maxId++;
            int id = this._maxId;
            List<Equipment> availableEquipment = new List<Equipment>();
            RoomType type = roomDTO.Type;
            int number = roomDTO.Number;
            bool isRenovating = roomDTO.IsRenovating;
            bool isActive = roomDTO.IsActive;

            Room room = new Room(id, type, number, isRenovating, availableEquipment, isActive);
            this.Rooms.Add(room);
            this.RoomById.Add(room.Id, room);
            Save();
            return room;
        }

        public void Update(int id, RoomDTO roomDTO)
        {
            Room room = GetById(id);
            room.Type = roomDTO.Type;
            room.Number = roomDTO.Number;
            room.IsRenovating = roomDTO.IsRenovating;
            room.IsActive = roomDTO.IsActive;
            Save();
        }


        public void Delete(int id)
        {
            Room room = GetById(id);
            this.Rooms.Remove(room);
            this.RoomById.Remove(id);
            Save();
        }

        public void AddToRoom(int id, Equipment equipment)
        {
            RoomById[id].AvailableEquipment.Add(equipment);
            Save();
        }

        public List<Room> GetActive()
        {
            List<Room> activeRooms = new List<Room>();
            foreach (Room room in this.Rooms)
            {
                if (room.IsActive)
                    activeRooms.Add(room);
            }
            return activeRooms;
        }

        public List<Room> GetNotRenovating()
        {
            List<Room> availableRooms = new List<Room>();
            foreach (Room room in this.Rooms)
            {
                if (room.IsActive && !room.IsRenovating)
                    availableRooms.Add(room);
            }
            return availableRooms;
        }

        public List<Equipment> GetDynamicEquipment(Room room)
        {
            List<Equipment> dynamicEquipment = new List<Equipment>();
            foreach (var equipment in room.AvailableEquipment)
            {
                if (equipment.IsDynamic)
                    dynamicEquipment.Add(equipment);
            }
            return dynamicEquipment;
        }
    }
}
