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

        private Renovation Parse(JToken? renovation)
        {
            Dictionary<int, Room> roomById = RoomRepository.GetInstance().RoomById;

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
            else if (renovation.GetType() == typeof(RoomMerger))
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

        public Renovation GetById(int id)
        {
            if (RenovationById.ContainsKey(id))
                return RenovationById[id];
            return null;
        }

        public void AddRenovation(RenovationDTO renovationDTO)
        {
            this._maxId++;
            int id = this._maxId;
            Room room = renovationDTO.Room;
            DateTime startDate = renovationDTO.StartDate;
            DateTime endDate = renovationDTO.EndDate;
            Renovation renovation = new Renovation(id, room, startDate, endDate);
            this.Renovations.Add(renovation);
            this.RenovationById.Add(renovation.Id, renovation);
            Save();
        }

        public void AddRoomMerger(RoomMergerDTO roomMergerDTO)
        {
            this._maxId++;
            int id = this._maxId;
            Room initialRoom = roomMergerDTO.Room;
            Room roomForMerge = roomMergerDTO.RoomForMerge;
            Room mergedRoom = roomMergerDTO.MergedRoom;
            DateTime startDate = roomMergerDTO.StartDate;
            DateTime endDate = roomMergerDTO.EndDate;
            Renovation renovation = new RoomMerger(id, initialRoom, roomForMerge, mergedRoom, startDate, endDate);
            this.Renovations.Add(renovation);
            this.RenovationById.Add(renovation.Id, renovation);
            Save();
        }

        public void AddRoomSeparation(RoomSeparationDTO roomSeparationDTO)
        {
            this._maxId++;
            int id = this._maxId;
            Room initialRoom = roomSeparationDTO.Room;
            Room firstRoom = roomSeparationDTO.FirstRoom;
            Room secondRoom = roomSeparationDTO.SecondRoom;
            DateTime startDate = roomSeparationDTO.StartDate;
            DateTime endDate = roomSeparationDTO.EndDate;
            Renovation renovation = new RoomSeparation(id, initialRoom, firstRoom, secondRoom, startDate, endDate);
            this.Renovations.Add(renovation);
            this.RenovationById.Add(renovation.Id, renovation);
            Save();
        }

        public void UpdateRenovation(int id, RenovationDTO renovationDTO)
        {
            Renovation renovation = GetById(id);
            renovation.Room = renovationDTO.Room;
            renovation.StartDate = renovationDTO.StartDate;
            renovation.EndDate = renovationDTO.EndDate;
            Save();
        }

        public void UpdateRoomMerger(int id, RoomMergerDTO roomMergerDTO)
        {
            RoomMerger renovation = (RoomMerger)GetById(id);
            renovation.Room = roomMergerDTO.Room;
            renovation.RoomForMerge = roomMergerDTO.RoomForMerge;
            renovation.MergedRoom = roomMergerDTO.MergedRoom;
            renovation.StartDate = roomMergerDTO.StartDate;
            renovation.EndDate = roomMergerDTO.EndDate;
            Save();
        }

        public void UpdateRoomSeparation(int id, RoomSeparationDTO roomSeparationDTO)
        {
            RoomSeparation renovation = (RoomSeparation)GetById(id);
            renovation.Room = roomSeparationDTO.Room;
            renovation.FirstRoom = roomSeparationDTO.FirstRoom;
            renovation.SecondRoom = roomSeparationDTO.SecondRoom;
            renovation.StartDate = roomSeparationDTO.StartDate;
            renovation.EndDate = roomSeparationDTO.EndDate;
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
