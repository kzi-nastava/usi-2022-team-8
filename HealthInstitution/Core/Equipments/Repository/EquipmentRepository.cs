using HealthInstitution.Core.Equipments.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Equipments.Repository
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private String _fileName = @"..\..\..\Data\equipments.json";

        private int _maxId;
        public List<Equipment> Equipments { get; set; }
        public Dictionary<int, Equipment> EquipmentById { get; set; }
        public Dictionary<string, int> EquipmentPerQuantity { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        public EquipmentRepository()
        {
            this.Equipments = new List<Equipment>();
            this.EquipmentById = new Dictionary<int, Equipment>();
            this.EquipmentPerQuantity = new Dictionary<string, int>();
            this._maxId = 0;
            this.LoadFromFile();
        }

        public void LoadFromFile()
        {
            var equipments = JsonSerializer.Deserialize<List<Equipment>>(File.ReadAllText(@"..\..\..\Data\equipments.json"), _options);
            foreach (Equipment equipment in equipments)
            {
                if (equipment.Id > _maxId)
                {
                    _maxId = equipment.Id;
                }
                this.Equipments.Add(equipment);
                this.EquipmentById.Add(equipment.Id, equipment);
            }
            FillTotalQuantityOfEquipment();
        }

        private void FillTotalQuantityOfEquipment()
        {
            foreach (Equipment equipment in Equipments)
            {
                if (equipment.IsDynamic)
                {
                    if (EquipmentPerQuantity.ContainsKey(equipment.Name))
                    {
                        EquipmentPerQuantity[equipment.Name] += equipment.Quantity;
                    }
                    else
                    {
                        EquipmentPerQuantity.Add(equipment.Name, equipment.Quantity);
                    }
                }
            }
        }

        public void Save()
        {
            var allEquipments = JsonSerializer.Serialize(this.Equipments, _options);
            File.WriteAllText(this._fileName, allEquipments);
        }

        public List<Equipment> GetAll()
        {
            return this.Equipments;
        }

        public Dictionary<int, Equipment> GetAllById()
        {
            return this.EquipmentById;
        }

        public Dictionary<string, int> GetAllByQuantity()
        {
            return this.EquipmentPerQuantity;
        }

        public Equipment GetById(int id)
        {
            if (EquipmentById.ContainsKey(id))
                return EquipmentById[id];
            return null;
        }

        public Equipment Add(Equipment equipment)
        {
            this._maxId++;
            int id = this._maxId;
            equipment.Id = id;

            this.Equipments.Add(equipment);
            this.EquipmentPerQuantity[equipment.Name] += equipment.Quantity;
            this.EquipmentById.Add(equipment.Id, equipment);
            Save();
            return equipment;
        }

        public void Update(int id, Equipment byEquipment)
        {
            Equipment equipment = GetById(id);
            equipment.Quantity = byEquipment.Quantity;
            equipment.Name = byEquipment.Name;
            equipment.Type = byEquipment.Type;
            equipment.IsDynamic = byEquipment.IsDynamic;
            Save();
        }

        public void Delete(int id)
        {
            Equipment equipment = GetById(id);
            this.Equipments.Remove(equipment);
            this.EquipmentById.Remove(id);
            Save();
        }

        public EquipmentType GetEquipmentType(string equipmentName)
        {
            EquipmentType equipmentType = EquipmentType.AppointmentEquipment;
            foreach (Equipment equipment in Equipments)
            {
                if (equipment.Name == equipmentName)
                    equipmentType = equipment.Type;
            }
            return equipmentType;
        }

        public void RemoveConsumed(Equipment equipment, int consumedQuantity)
        {
            equipment.Quantity -= consumedQuantity;
            EquipmentById[equipment.Id] = equipment;
            Save();
        }

        public Dictionary<String, int> GetEquipmentPerQuantity()
        {
            return EquipmentPerQuantity;
        }
    }
}