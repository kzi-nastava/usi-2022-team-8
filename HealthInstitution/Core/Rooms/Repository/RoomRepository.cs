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

        private List<Equipment> convertJTokenToEquipments(JToken tokens)
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
        public void LoadFromFile()
        {
           
            var rooms = JArray.Parse(File.ReadAllText(_fileName));
            //var rooms = JsonSerializer.Deserialize<List<Room>>(File.ReadAllText(@"..\..\..\Data\JSON\rooms.json"), _options);
            foreach (var room in rooms)
            {
                
                int id = (int)room["id"];
                RoomType type;
                Enum.TryParse(room["type"].ToString(), out type);
                int number = (int)room["number"];
                bool isRenovating = (bool)room["isRenovating"];
                List<Equipment> availableEquipment = convertJTokenToEquipments(room["availableEquipment"]);

                Room roomTemp = new Room(id,type,number,isRenovating,availableEquipment);

                if (id > _maxId)
                {
                    _maxId = id;
                }

                this.Rooms.Add(roomTemp);
                this.RoomById.Add(roomTemp.Id, roomTemp);
            }
        }

        private List<int> formListOfIds(List<Equipment> equipments)
        {
            var ids = new List<int>();
            foreach(var equipment in equipments)
            {
                ids.Add(equipment.Id);
            }
            return ids;
        }
        private List<dynamic> shortenRoom()
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
                    availableEquipment = formListOfIds(room.AvailableEquipment)
                });
            }
            return reducedRooms;
        }
        public void Save()
        {
            var allRooms = JsonSerializer.Serialize(shortenRoom(), _options);
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

        public void AddRoom(RoomType type, int number, bool isRenovating=false)
        {

            this._maxId++;
            int id = this._maxId;
            List<Equipment> availableEquipment = new List<Equipment>();
            Room room = new Room(id, type, number, isRenovating, availableEquipment);
            this.Rooms.Add(room);
            this.RoomById.Add(room.Id, room);
            Save();
        }

        public void Update(int id, RoomType type, int number, bool isRenovating)
        {
            Room room = GetById(id);
            room.Type = type;
            room.Number = number;
            room.IsRenovating = isRenovating;
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

    }
}
