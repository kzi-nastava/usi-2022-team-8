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
    public class RenovationRepository : IRenovationRepository
    {
        private String _fileName = @"..\..\..\Data\renovations.json";

        private IRoomRepository _roomRepository;

        private int _maxId;
        public List<Renovation> Renovations { get; set; }
        public Dictionary<int, Renovation> RenovationById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        public RenovationRepository(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
            this.Renovations = new List<Renovation>();
            this.RenovationById = new Dictionary<int, Renovation>();
            this._maxId = 0;
            this.LoadFromFile();
        }

        private Renovation Parse(JToken? renovation)
        {
            Dictionary<int, Room> roomById = _roomRepository.GetAllById();

            int id = (int)renovation["id"];
            int roomId = (int)renovation["room"];
            Room room = roomById[roomId];
            DateTime startDate = (DateTime)renovation["startDate"];
            DateTime endDate = (DateTime)renovation["endDate"];
            String type = (String)renovation["type"];

            if (type.Equals("simple"))
            {
                return new Renovation(id, room, startDate, endDate);
            }
            else if (type.Equals("merger"))
            {
                int roomForMergeId = (int)renovation["roomForMerge"];
                Room roomForMerge = roomById[roomForMergeId];
                int mergedRoomId = (int)renovation["mergedRoom"];
                Room mergedRoom = roomById[mergedRoomId];
                return new RoomMerger(id, room, roomForMerge, mergedRoom, startDate, endDate);
            }
            else
            {
                int firstRoomId = (int)renovation["firstRoom"];
                Room firstRoom = roomById[firstRoomId];
                int secondRoomId = (int)renovation["secondRoom"];
                Room secondRoom = roomById[secondRoomId];
                return new RoomSeparation(id, room, firstRoom, secondRoom, startDate, endDate);
            }
        }

        public void LoadFromFile()
        {
            var renovations = JArray.Parse(File.ReadAllText(_fileName));

            foreach (var renovation in renovations)
            {
                Renovation loadedRenovation = Parse(renovation);
                int id = loadedRenovation.Id;
                if (id > _maxId)
                {
                    _maxId = id;
                }

                this.Renovations.Add(loadedRenovation);
                this.RenovationById.Add(id, loadedRenovation);
            }
        }

        private List<dynamic> PrepareForSerialization()
        {
            List<dynamic> reducedRenovation = new List<dynamic>();
            foreach (Renovation renovation in this.Renovations)
            {
                AttachReducedRenovation(reducedRenovation, renovation);
            }
            return reducedRenovation;
        }

        private void AttachReducedRenovation(List<dynamic> reducedRenovation, Renovation renovation)
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
            }
            else if (renovation.IsRoomMerger())
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
            }
            else
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

        public void Save()
        {
            var allRenovations = JsonSerializer.Serialize(PrepareForSerialization(), _options);
            File.WriteAllText(this._fileName, allRenovations);
        }

        public List<Renovation> GetAll()
        {
            return this.Renovations;
        }

        public Dictionary<int, Renovation> GetAllById()
        {
            return this.RenovationById;
        }

        public Renovation GetById(int id)
        {
            if (RenovationById.ContainsKey(id))
                return RenovationById[id];
            return null;
        }

        public void AddRenovation(Renovation renovation)
        {
            this._maxId++;
            int id = this._maxId;
            renovation.Id = id;

            this.Renovations.Add(renovation);
            this.RenovationById.Add(renovation.Id, renovation);
            Save();
        }

        public void UpdateRenovation(int id, Renovation byRenovation)
        {
            Renovation renovation = GetById(id);
            renovation.Room = byRenovation.Room;
            renovation.StartDate = byRenovation.StartDate;
            renovation.EndDate = byRenovation.EndDate;
            Save();
        }

        public void UpdateRoomMerger(int id, RoomMerger byRoomMerger)
        {
            RoomMerger renovation = (RoomMerger)GetById(id);
            renovation.Room = byRoomMerger.Room;
            renovation.RoomForMerge = byRoomMerger.RoomForMerge;
            renovation.MergedRoom = byRoomMerger.MergedRoom;
            renovation.StartDate = byRoomMerger.StartDate;
            renovation.EndDate = byRoomMerger.EndDate;
            Save();
        }

        public void UpdateRoomSeparation(int id, RoomSeparation byRoomSeparation)
        {
            RoomSeparation renovation = (RoomSeparation)GetById(id);
            renovation.Room = byRoomSeparation.Room;
            renovation.FirstRoom = byRoomSeparation.FirstRoom;
            renovation.SecondRoom = byRoomSeparation.SecondRoom;
            renovation.StartDate = byRoomSeparation.StartDate;
            renovation.EndDate = byRoomSeparation.EndDate;
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