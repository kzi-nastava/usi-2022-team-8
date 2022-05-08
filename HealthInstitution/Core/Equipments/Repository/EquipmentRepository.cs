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
    public class EquipmentRepository
    {
        private String _fileName;

        private int _maxId;
        public List<Equipment> Equipments { get; set; }
        public Dictionary<int, Equipment> EquipmentById { get; set; }

        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private EquipmentRepository(String fileName)
        {
            this._fileName = fileName;
            this.Equipments = new List<Equipment>();
            this.EquipmentById = new Dictionary<int, Equipment>();
            this._maxId = 0;
            this.LoadFromFile();
        }
        private static EquipmentRepository s_instance = null;
        public static EquipmentRepository GetInstance()
        {
            {
                if (s_instance == null)
                {
                    s_instance = new EquipmentRepository(@"..\..\..\Data\JSON\equipments.json");
                }
                return s_instance;
            }
        }
        public void LoadFromFile()
        {
            var equipments = JsonSerializer.Deserialize<List<Equipment>>(File.ReadAllText(@"..\..\..\Data\JSON\equipments.json"), _options);
            foreach (Equipment equipment in equipments)
            {
                if (equipment.Id > _maxId)
                {
                    _maxId = equipment.Id;
                }
                this.Equipments.Add(equipment);
                this.EquipmentById.Add(equipment.Id, equipment);
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

        public Equipment GetById(int id)
        {
            if (EquipmentById.ContainsKey(id))
                return EquipmentById[id];
            return null;
        }

        public Equipment Add(int quantity, string name, EquipmentType type, bool isDynamic)
        {

            this._maxId++;
            int id = this._maxId;
            Equipment equipment = new Equipment(id, quantity, name, type, isDynamic);
            this.Equipments.Add(equipment);
            this.EquipmentById.Add(equipment.Id, equipment);
            Save();
            return equipment;
        }

        public void Update(int id, int quantity, string name, EquipmentType type, bool isDynamic)
        {
            Equipment equipment = GetById(id);
            equipment.Quantity = quantity;
            equipment.Name = name;
            equipment.Type = type;
            equipment.IsDynamic = isDynamic;
            Save();
        }
        public void Delete(int id)
        {
            Equipment equipment = GetById(id);
            this.Equipments.Remove(equipment);
            this.EquipmentById.Remove(id);
            Save();
        }
    }
}

