using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.Examinations.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.GUI.ManagerView;
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

        public Room AddRoom(Room room)
        {
            this._maxId++;
            int id = this._maxId;
            room.Id = id;
            this.Rooms.Add(room);
            this.RoomById.Add(room.Id, room);
            Save();
            return room;
        }

        public void Update(int id, Room byRoom)
        {
            Room room = GetById(id);
            room.Type = byRoom.Type;
            room.Number = byRoom.Number;
            room.IsRenovating = byRoom.IsRenovating;
            room.IsActive = byRoom.IsActive;
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

        public Room FindAvailableRoom(DateTime appointment)
        {
            List<Room> availableRooms = FindAllAvailableRooms(appointment);

            if (availableRooms.Count == 0) throw new Exception("There are no available rooms!");

            Random random = new Random();
            int index = random.Next(0, availableRooms.Count);
            return availableRooms[index];
        }

        public List<Room> FindAllAvailableRooms(DateTime appointment)
        {
            bool isAvailable;
            List<Room> availableRooms = new List<Room>();
            foreach (var room in RoomRepository.GetInstance().GetNotRenovating())
            {
                if (room.Type != RoomType.ExaminationRoom) continue;
                isAvailable = true;
                foreach (var examination in ExaminationRepository.GetInstance().Examinations)
                {
                    if (examination.Appointment == appointment && examination.Room.Id == room.Id)
                    {
                        isAvailable = false;
                        break;
                    }
                }
                if (isAvailable)
                    availableRooms.Add(room);
            }
            return availableRooms;
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
        public Room? GetRoomFromString(string? roomFromForm)
        {
            if (roomFromForm != null)
            {
                string[] tokens = roomFromForm.Split(' ');
                string type = tokens[0], number = tokens[1];
                foreach (Room room in Rooms)
                {
                    if (room.Type.ToString() == type && room.Number.ToString() == number)
                        return room;
                }
            }
            return null;
        }

        public bool RoomNumberIsTaken(int number)
        {
            return this.Rooms.Any(room => room.Number == number);
        }

        public int FindIndexWithRoomNumber(int number)
        {
            return this.Rooms.FindIndex(room => room.Number == number);
        }

    }
}
