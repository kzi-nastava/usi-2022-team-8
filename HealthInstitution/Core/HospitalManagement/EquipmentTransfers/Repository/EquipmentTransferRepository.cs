using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.EquipmentTransfers.Model;
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

namespace HealthInstitution.Core.EquipmentTransfers.Repository
{
    public class EquipmentTransferRepository : IEquipmentTransferRepository
    {
        private String _fileName = @"..\..\..\Data\equipmentTransfers.json";

        private IEquipmentRepository _equipmentRepository;
        private IRoomRepository _roomRepository;

        private int _maxId;
        public List<EquipmentTransfer> EquipmentTransfers { get; set; }
        public Dictionary<int, EquipmentTransfer> EquipmentTransferById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        public EquipmentTransferRepository(IEquipmentRepository equipmentRepository, IRoomRepository roomRepository)
        {
            _equipmentRepository = equipmentRepository;
            _roomRepository = roomRepository;
            this.EquipmentTransfers = new List<EquipmentTransfer>();
            this.EquipmentTransferById = new Dictionary<int, EquipmentTransfer>();
            this._maxId = 0;
            this.LoadFromFile();
        }

        private EquipmentTransfer Parse(JToken? equipmentTransfer)
        {
            Dictionary<int, Equipment> equipmentById = _equipmentRepository.GetAllById();
            Dictionary<int, Room> roomById = _roomRepository.GetAllById();

            int id = (int)equipmentTransfer["id"];
            int equipmentId = (int)equipmentTransfer["equipment"];
            Equipment equipment = equipmentById[equipmentId];
            int fromRoomId = (int)equipmentTransfer["fromRoom"];
            Room fromRoom = (fromRoomId == 0) ? null : roomById[fromRoomId];
            int toRoomId = (int)equipmentTransfer["toRoom"];
            Room toRoom = roomById[toRoomId];
            DateTime transferTime = (DateTime)equipmentTransfer["transferTime"];

            return new EquipmentTransfer(id, equipment, fromRoom, toRoom, transferTime);
        }

        public void LoadFromFile()
        {
            var equipmentTransfers = JArray.Parse(File.ReadAllText(_fileName));

            foreach (var equipmentTransfer in equipmentTransfers)
            {
                EquipmentTransfer loadedEquipmentTransfer = Parse(equipmentTransfer);
                int id = loadedEquipmentTransfer.Id;

                if (id > _maxId)
                {
                    _maxId = id;
                }

                this.EquipmentTransfers.Add(loadedEquipmentTransfer);
                this.EquipmentTransferById[id] = loadedEquipmentTransfer;
            }
        }

        private List<dynamic> PrepareForSerialization()
        {
            List<dynamic> reducedEquipmentTransfers = new List<dynamic>();
            foreach (var equipmentTransfer in this.EquipmentTransfers)
            {
                reducedEquipmentTransfers.Add(new
                {
                    id = equipmentTransfer.Id,
                    equipment = equipmentTransfer.Equipment.Id,
                    fromRoom = (equipmentTransfer.FromRoom == null) ? 0 : equipmentTransfer.FromRoom.Id,
                    toRoom = equipmentTransfer.ToRoom.Id,
                    transferTime = equipmentTransfer.TransferTime
                });
            }
            return reducedEquipmentTransfers;
        }

        public void Save()
        {
            var allEquipmentTransfers = JsonSerializer.Serialize(PrepareForSerialization(), _options);
            File.WriteAllText(this._fileName, allEquipmentTransfers);
        }

        public List<EquipmentTransfer> GetAll()
        {
            return this.EquipmentTransfers;
        }

        public Dictionary<int, EquipmentTransfer> GetAllById()
        {
            return this.EquipmentTransferById;
        }

        public EquipmentTransfer GetById(int id)
        {
            if (EquipmentTransferById.ContainsKey(id))
                return EquipmentTransferById[id];
            return null;
        }

        public void Add(EquipmentTransfer equipmentTransfer)
        {
            this._maxId++;
            int id = this._maxId;
            equipmentTransfer.Id = id;

            this.EquipmentTransfers.Add(equipmentTransfer);
            this.EquipmentTransferById.Add(equipmentTransfer.Id, equipmentTransfer);
            Save();
        }

        public void Update(int id, EquipmentTransfer byEquipmentTransfer)
        {
            EquipmentTransfer equipmentTransfer = GetById(id);
            equipmentTransfer.Equipment = byEquipmentTransfer.Equipment;
            equipmentTransfer.FromRoom = byEquipmentTransfer.FromRoom;
            equipmentTransfer.ToRoom = byEquipmentTransfer.ToRoom;
            equipmentTransfer.TransferTime = byEquipmentTransfer.TransferTime;
            Save();
        }

        public void Delete(int id)
        {
            EquipmentTransfer equipmentTransfer = GetById(id);
            this.EquipmentTransfers.Remove(equipmentTransfer);
            this.EquipmentTransferById.Remove(id);
            Save();
        }
    }
}