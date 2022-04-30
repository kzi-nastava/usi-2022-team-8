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
        public String fileName { get; set; }

        private int maxId;
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
            this.maxId = 0;
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

        public List<Equipment> ConvertJTokenToEquipments(JToken tokens)
        {
            var equipmentById = EquipmentRepository.GetInstance().equipmentById;
            List<Equipment> equipments = new List<Equipment>();
            foreach (JToken equipmentToken in tokens)
            {
                Equipment equipment = equipmentById[(int)equipmentToken];
                equipments.Add(equipment);
            }
            return equipments;
        }
        public void LoadRooms()
        {
           
            var rooms = JArray.Parse(File.ReadAllText(fileName));
            //var rooms = JsonSerializer.Deserialize<List<Room>>(File.ReadAllText(@"..\..\..\Data\JSON\rooms.json"), options);
            foreach (var room in rooms)
            {
                
                int id = (int)room["id"];
                RoomType type;
                Enum.TryParse(room["type"].ToString(), out type);
                int number = (int)room["number"];
                bool isRenovating = (bool)room["isRenovating"];
                List<Equipment> availableEquipment = ConvertJTokenToEquipments(room["availableEquipment"]);

                Room roomTemp = new Room(id,type,number,isRenovating,availableEquipment);

                if (id > maxId)
                {
                    maxId = id;
                }

                this.rooms.Add(roomTemp);
                this.roomById.Add(roomTemp.id, roomTemp);
            }
        }

        public List<int> FormListOfIds(List<Equipment> equipments)
        {
            var ids = new List<int>();
            foreach(var equipment in equipments)
            {
                ids.Add(equipment.id);
            }
            return ids;
        }
        public List<dynamic> ShortenRoom()
        {
            List<dynamic> reducedRooms = new List<dynamic>();
            foreach (var room in this.rooms)
            {
                reducedRooms.Add(new
                {
                    id = room.id,
                    type = room.type,
                    number = room.number,
                    isRenovating = room.isRenovating,
                    availableEquipment = FormListOfIds(room.availableEquipment)
                });
            }
            return reducedRooms;
        }
        public void SaveRooms()
        {
            var allRooms = JsonSerializer.Serialize(ShortenRoom(), options);
            File.WriteAllText(this.fileName, allRooms);
        }

        public List<Room> GetRooms()
        {
            return this.rooms;
        }

        public Room GetRoomById(int id)
        {
            if (roomById.ContainsKey(id))
                return roomById[id];
            return null;
        }

        public void AddRoom(RoomType type, int number, bool isRenovating=false)
        {

            this.maxId++;
            int id = this.maxId;
            List<Equipment> availableEquipment = new List<Equipment>();
            Room room = new Room(id, type, number, isRenovating, availableEquipment);
            this.rooms.Add(room);
            this.roomById.Add(room.id, room);
            SaveRooms();
        }

        public void UpdateRoom(int id, RoomType type, int number, bool isRenovating)
        {
            Room room = GetRoomById(id);
            room.type = type;
            room.number = number;
            room.isRenovating = isRenovating;
            SaveRooms();
        }


        public void DeleteRoom(int id)
        {
            Room room = GetRoomById(id);
            this.rooms.Remove(room);
            this.roomById.Remove(id);
            SaveRooms();
        }

        public void AddEquipmentToRoom(int id, Equipment equipment)
        {
            roomById[id].availableEquipment.Add(equipment);
            SaveRooms();
        }

    }
}
