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
        public String fileName { get; set; }

        private int maxId;
        public List<Equipment> equipments { get; set; }
        public Dictionary<int, Equipment> equipmentById { get; set; }

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        private EquipmentRepository(String fileName)
        {
            this.fileName = fileName;
            this.equipments = new List<Equipment>();
            this.equipmentById = new Dictionary<int, Equipment>();
            this.maxId = 0;
            this.LoadEquipments();
        }
        private static EquipmentRepository instance = null;
        public static EquipmentRepository GetInstance()
        {
            {
                if (instance == null)
                {
                    instance = new EquipmentRepository(@"..\..\..\Data\JSON\equipments.json");
                }
                return instance;
            }
        }
        public void LoadEquipments()
        {
            var equipments = JsonSerializer.Deserialize<List<Equipment>>(File.ReadAllText(@"..\..\..\Data\JSON\equipments.json"), options);
            foreach (Equipment equipment in equipments)
            {
                if (equipment.id > maxId)
                {
                    maxId = equipment.id;
                }
                this.equipments.Add(equipment);
                this.equipmentById.Add(equipment.id, equipment);
            }
        }

        public void SaveEquipments()
        {
            var allEquipments = JsonSerializer.Serialize(this.equipments, options);
            File.WriteAllText(this.fileName, allEquipments);
        }

        public List<Equipment> GetEquipments()
        {
            return this.equipments;
        }

        public Equipment GetEquipmentById(int id)
        {
            if (equipmentById.ContainsKey(id))
                return equipmentById[id];
            return null;
        }

        public void AddEquipment(int quantity, string name, EquipmentType type, bool isDynamic)
        {

            this.maxId++;
            int id = this.maxId;

            Equipment equipment = new Equipment(id, quantity, name, type, isDynamic);
            this.equipments.Add(equipment);
            this.equipmentById.Add(equipment.id, equipment);
            SaveEquipments();
        }

        public void UpdateEquipment(int id, int quantity, string name, EquipmentType type, bool isDynamic)
        {
            Equipment equipment = GetEquipmentById(id);
            equipment.quantity = quantity;
            equipment.name = name;
            equipment.type = type;
            equipment.isDynamic = isDynamic;
            SaveEquipments();
        }


        public void DeleteEquipment(int id)
        {
            Equipment equipment = GetEquipmentById(id);
            this.equipments.Remove(equipment);
            this.equipmentById.Remove(id);
            SaveEquipments();
        }
    }
}

