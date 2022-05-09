using HealthInstitution.Core.Renovations.Model;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Renovations.Repository
{
    public class RenovationRepository
    {
        private String _fileName;

        private int _maxId;
        public List<Renovation> Renovations { get; set; }
        public Dictionary<int, Renovation> RenovationById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
        private RenovationRepository(String fileName)
        {
            this._fileName = fileName;
            this.Renovations = new List<Renovation>();
            this.RenovationById = new Dictionary<int, Renovation>();
            this._maxId = 0;
            this.LoadFromFile();
        }
        private static RenovationRepository s_instance = null;
        public static RenovationRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new RenovationRepository(@"..\..\..\Data\JSON\renovations.json");
                }
                return s_instance;
            }
        }

        public void LoadFromFile()
        {
            var roomById = RoomRepository.GetInstance().RoomById;
            var renovations = JArray.Parse(File.ReadAllText(_fileName));
            //var equipmentTransfers = JsonSerializer.Deserialize<List<Room>>(File.ReadAllText(@"..\..\..\Data\JSON\renovations.json"), _options);
            foreach (var renovation in renovations)
            {
                Renovation renovationTemp;

                int id = (int)renovation["id"];
                int roomId = (int)renovation["room"];
                Room room = roomById[roomId];
                DateTime startDate = (DateTime)renovation["startDate"];
                DateTime endDate = (DateTime)renovation["endDate"];
                String type = (String)renovation["type"];

                if (type.Equals("simple"))
                {
                    renovationTemp = new Renovation(id, room, startDate, endDate);
                } else if (type.Equals("merger"))
                {
                    int roomForMergeId = (int)renovation["roomForMerge"];
                    Room roomForMerge = roomById[roomForMergeId];
                    int mergedRoomId = (int)renovation["mergedRoom"];
                    Room mergedRoom = roomById[mergedRoomId];
                    renovationTemp = new RoomMerger(id, room, roomForMerge, mergedRoom, startDate, endDate);
                } else
                {
                    int firstRoomId = (int)renovation["firstRoom"];
                    Room firstRoom = roomById[firstRoomId];
                    int secondRoomId = (int)renovation["secondRoom"];
                    Room secondRoom = roomById[secondRoomId];
                    renovationTemp = new RoomSeparation(id, room, firstRoom, secondRoom, startDate, endDate);
                }
                
                if (id > _maxId)
                {
                    _maxId = id;
                }

                this.Renovations.Add(renovationTemp);
                this.RenovationById.Add(renovationTemp.Id, renovationTemp);
            }
        }

        private List<dynamic> ShortenRenovation()
        {
            List<dynamic> reducedRenovation = new List<dynamic>();
            foreach (Renovation renovation in this.Renovations)
            {
                if (renovation.GetType() == typeof(Renovation))
                {
                    reducedRenovation.Add(new
                    {
                        id = renovation.Id,
                        room = renovation.Room.Id,
                        startDate = renovation.StartDate,
                        endDate = renovation.EndDate,
                        type = "simple"
                    });
                } else if (renovation.GetType() == typeof(RoomMerger))
                {
                    RoomMerger roomMerger = (RoomMerger)renovation;
                    reducedRenovation.Add(new
                    {
                        id = roomMerger.Id,
                        room = roomMerger.Room.Id,
                        startDate = roomMerger.StartDate,
                        endDate = roomMerger.EndDate,
                        roomForMerge = roomMerger.RoomForMerge.Id,
                        mergedRoom = roomMerger.MergedRoom.Id,
                        type = "merger"
                    });
                } else
                {
                    RoomSeparation roomSeparation = (RoomSeparation)renovation;
                    reducedRenovation.Add(new
                    {
                        id = roomSeparation.Id,
                        room = roomSeparation.Room.Id,
                        startDate = roomSeparation.StartDate,
                        endDate = roomSeparation.EndDate,
                        firstRoom = roomSeparation.FirstRoom.Id,
                        secondRoom = roomSeparation.SecondRoom.Id,
                        type = "separation"
                    });
                }
                
            }
            return reducedRenovation;
        }
        public void Save()
        {
            var allRenovations = JsonSerializer.Serialize(ShortenRenovation(), _options);
            File.WriteAllText(this._fileName, allRenovations);
        }

        public List<Renovation> GetAll()
        {
            return this.Renovations;
        }

        public Renovation GetById(int id)
        {
            if (RenovationById.ContainsKey(id))
                return RenovationById[id];
            return null;
        }

        public void AddRenovation(Room room, DateTime startDate, DateTime endDate)
        {
            this._maxId++;
            int id = this._maxId;
            Renovation renovation = new Renovation(id, room, startDate, endDate);
            this.Renovations.Add(renovation);
            this.RenovationById.Add(renovation.Id, renovation);
            Save();
        }

        public void AddRoomMerger(Room initialRoom, Room roomForMerge, Room mergedRoom, DateTime startDate, DateTime endDate)
        {
            this._maxId++;
            int id = this._maxId;
            Renovation renovation = new RoomMerger(id, initialRoom, roomForMerge, mergedRoom, startDate, endDate);
            this.Renovations.Add(renovation);
            this.RenovationById.Add(renovation.Id, renovation);
            Save();
        }

        public void AddRoomSeparation(Room initialRoom, Room firstRoom, Room secondRoom, DateTime startDate, DateTime endDate)
        {
            this._maxId++;
            int id = this._maxId;
            Renovation renovation = new RoomSeparation(id, initialRoom, firstRoom, secondRoom, startDate, endDate);
            this.Renovations.Add(renovation);
            this.RenovationById.Add(renovation.Id, renovation);
            Save();
        }

        public void UpdateRenovation(int id, Room room, DateTime startDate, DateTime endDate)
        {
            Renovation renovation = GetById(id);
            renovation.Room = room;
            renovation.StartDate = startDate;
            renovation.EndDate = endDate;
            Save();
        }

        public void UpdateRoomMerger(int id, Room initialRoom, Room roomForMerge, Room mergedRoom, DateTime startDate, DateTime endDate)
        {
            RoomMerger renovation = (RoomMerger)GetById(id);
            renovation.Room = initialRoom;
            renovation.RoomForMerge = roomForMerge;
            renovation.MergedRoom = mergedRoom;
            renovation.StartDate = startDate;
            renovation.EndDate = endDate;
            Save();
        }

        public void UpdateRoomSeparation(int id, Room initialRoom, Room firstRoom, Room secondRoom, DateTime startDate, DateTime endDate)
        {
            RoomSeparation renovation = (RoomSeparation)GetById(id);
            renovation.Room = initialRoom;
            renovation.FirstRoom = firstRoom;
            renovation.SecondRoom = secondRoom;
            renovation.StartDate = startDate;
            renovation.EndDate = endDate;
            Save();
        }


        public void Delete(int id)
        {
            Renovation renovation = GetById(id);
            this.Renovations.Remove(renovation);
            this.RenovationById.Remove(id);
            Save();
        }

    }
}
