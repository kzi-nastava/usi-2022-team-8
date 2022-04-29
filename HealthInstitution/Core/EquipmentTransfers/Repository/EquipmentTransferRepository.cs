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
    public class EquipmentTransferRepository
    {
        public String fileName { get; set; }

        private int maxId;
        public List<EquipmentTransfer> equipmentTransfers { get; set; }
        public Dictionary<int, EquipmentTransfer> equipmentTransferById { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private EquipmentTransferRepository(String fileName)
        {
            this.fileName = fileName;
            this.equipmentTransfers = new List<EquipmentTransfer>();
            this.equipmentTransferById = new Dictionary<int, EquipmentTransfer>();
            this.maxId = 0;
            this.LoadEquipmentTransfers();
        }
        private static EquipmentTransferRepository instance = null;
        public static EquipmentTransferRepository GetInstance()
        {
            {
                if (instance == null)
                {
                    instance = new EquipmentTransferRepository(@"..\..\..\Data\JSON\equipmentTransfers.json");
                }
                return instance;
            }
        }

        public void LoadEquipmentTransfers()
        {
            var equipmentById = EquipmentRepository.GetInstance().equipmentById;
            var roomById = RoomRepository.GetInstance().roomById;
            var equipmentTransfers = JArray.Parse(File.ReadAllText(fileName));
            //var equipmentTransfers = JsonSerializer.Deserialize<List<Room>>(File.ReadAllText(@"..\..\..\Data\JSON\equipmentTransfers.json"), options);
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

                if (id > maxId)
                {
                    maxId = id;
                }

                this.equipmentTransfers.Add(eqTransferTemp);
                this.equipmentTransferById.Add(eqTransferTemp.id, eqTransferTemp);
            }
        }

        public List<dynamic> ShortenEquipmentTransfer()
        {
            List<dynamic> reducedEquipmentTransfers = new List<dynamic>();
            foreach (var equipmentTransfer in this.equipmentTransfers)
            {
                reducedEquipmentTransfers.Add(new
                {
                    id = equipmentTransfer.id,
                    equipment = equipmentTransfer.equipment.id,
                    fromRoom = equipmentTransfer.fromRoom.id,
                    toRoom = equipmentTransfer.toRoom.id,
                    transferTime = equipmentTransfer.transferTime
                });
            }
            return reducedEquipmentTransfers;
        }
        public void SaveEquipmentTransfers()
        {
            var allEquipmentTransfers = JsonSerializer.Serialize(ShortenEquipmentTransfer(), options);
            File.WriteAllText(this.fileName, allEquipmentTransfers);
        }

        public List<EquipmentTransfer> GetEquipmentTransfers()
        {
            return this.equipmentTransfers;
        }

        public EquipmentTransfer GetEquipmentTransferById(int id)
        {
            if (equipmentTransferById.ContainsKey(id))
                return equipmentTransferById[id];
            return null;
        }

        public void AddEquipmentTransfer(Equipment equipment, Room fromRoom, Room toRoom, DateTime transferTime)
        {
            this.maxId++;
            int id = this.maxId;
            EquipmentTransfer equipmentTransfer = new EquipmentTransfer(id,equipment,fromRoom,toRoom,transferTime);
            this.equipmentTransfers.Add(equipmentTransfer);
            this.equipmentTransferById.Add(equipmentTransfer.id, equipmentTransfer);
            SaveEquipmentTransfers();
        }

        public void UpdateEquipmentTransfer(int id,Equipment equipment, Room fromRoom, Room toRoom, DateTime transferTime)
        {
            EquipmentTransfer equipmentTransfer = GetEquipmentTransferById(id);
            equipmentTransfer.equipment = equipment;
            equipmentTransfer.fromRoom = fromRoom;
            equipmentTransfer.toRoom = toRoom;
            equipmentTransfer.transferTime = transferTime;
            
            SaveEquipmentTransfers();
        }


        public void DeleteEquipmentTransfer(int id)
        {
            EquipmentTransfer equipmentTransfer = GetEquipmentTransferById(id);
            this.equipmentTransfers.Remove(equipmentTransfer);
            this.equipmentTransferById.Remove(id);
            SaveEquipmentTransfers();
        }

    }
}
