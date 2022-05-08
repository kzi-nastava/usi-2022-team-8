﻿using HealthInstitution.Core.Equipments.Model;
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
    public class EquipmentTransferRepository
    {
        private String _fileName;

        private int _maxId;
        public List<EquipmentTransfer> EquipmentTransfers { get; set; }
        public Dictionary<int, EquipmentTransfer> EquipmentTransferById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
        private EquipmentTransferRepository(String fileName)
        {
            this._fileName = fileName;
            this.EquipmentTransfers = new List<EquipmentTransfer>();
            this.EquipmentTransferById = new Dictionary<int, EquipmentTransfer>();
            this._maxId = 0;
            this.LoadFromFile();
        }
        private static EquipmentTransferRepository s_instance = null;
        public static EquipmentTransferRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new EquipmentTransferRepository(@"..\..\..\Data\JSON\equipmentTransfers.json");
                }
                return s_instance;
            }
        }

        public void LoadFromFile()
        {
            var equipmentById = EquipmentRepository.GetInstance().EquipmentById;
            var roomById = RoomRepository.GetInstance().RoomById;
            var equipmentTransfers = JArray.Parse(File.ReadAllText(_fileName));
            //var equipmentTransfers = JsonSerializer.Deserialize<List<Room>>(File.ReadAllText(@"..\..\..\Data\JSON\equipmentTransfers.json"), _options);
            foreach (var equipmentTransfer in equipmentTransfers)
            {

                int id = (int)equipmentTransfer["id"];
                int equipmentId = (int)equipmentTransfer["equipment"];
                Equipment equipment = equipmentById[equipmentId];
                int fromRoomId = (int)equipmentTransfer["fromRoom"];
                Room fromRoom = roomById[fromRoomId];
                int toRoomId = (int)equipmentTransfer["toRoom"];
                Room toRoom = roomById[toRoomId];
                DateTime transferTime = (DateTime)equipmentTransfer["transferTime"];

                EquipmentTransfer eqTransferTemp = new EquipmentTransfer(id, equipment, fromRoom, toRoom, transferTime);

                if (id > _maxId)
                {
                    _maxId = id;
                }

                this.EquipmentTransfers.Add(eqTransferTemp);
                this.EquipmentTransferById.Add(eqTransferTemp.Id, eqTransferTemp);
            }
        }

        private List<dynamic> ShortenEquipmentTransfer()
        {
            List<dynamic> reducedEquipmentTransfers = new List<dynamic>();
            foreach (var equipmentTransfer in this.EquipmentTransfers)
            {
                reducedEquipmentTransfers.Add(new
                {
                    id = equipmentTransfer.Id,
                    equipment = equipmentTransfer.Equipment.Id,
                    fromRoom = equipmentTransfer.FromRoom.Id,
                    toRoom = equipmentTransfer.ToRoom.Id,
                    transferTime = equipmentTransfer.TransferTime
                });
            }
            return reducedEquipmentTransfers;
        }
        public void Save()
        {
            var allEquipmentTransfers = JsonSerializer.Serialize(ShortenEquipmentTransfer(), _options);
            File.WriteAllText(this._fileName, allEquipmentTransfers);
        }

        public List<EquipmentTransfer> GetAll()
        {
            return this.EquipmentTransfers;
        }

        public EquipmentTransfer GetById(int id)
        {
            if (EquipmentTransferById.ContainsKey(id))
                return EquipmentTransferById[id];
            return null;
        }

        public void Add(Equipment equipment, Room fromRoom, Room toRoom, DateTime transferTime)
        {
            this._maxId++;
            int id = this._maxId;
            EquipmentTransfer equipmentTransfer = new EquipmentTransfer(id,equipment,fromRoom,toRoom,transferTime);
            this.EquipmentTransfers.Add(equipmentTransfer);
            this.EquipmentTransferById.Add(equipmentTransfer.Id, equipmentTransfer);
            Save();
        }

        public void Update(int id,Equipment equipment, Room fromRoom, Room toRoom, DateTime transferTime)
        {
            EquipmentTransfer equipmentTransfer = GetById(id);
            equipmentTransfer.Equipment = equipment;
            equipmentTransfer.FromRoom = fromRoom;
            equipmentTransfer.ToRoom = toRoom;
            equipmentTransfer.TransferTime = transferTime;
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
