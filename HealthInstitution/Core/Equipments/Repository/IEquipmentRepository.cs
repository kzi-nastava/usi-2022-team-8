using HealthInstitution.Core.Equipments.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Equipments.Repository
{
    public interface IEquipmentRepository
    {
        public void LoadFromFile();
        public void Save();
        public List<Equipment> GetAll();

        public Dictionary<int, Equipment> GetAllById();

        public Dictionary<string, int> GetAllByQuantity();
        public Equipment GetById(int id);
        public Equipment Add(Equipment equipment);
        public void Update(int id, Equipment byEquipment);
        public void Delete(int id);
        public EquipmentType GetEquipmentType(string equipmentName);
        public void RemoveConsumed(Equipment equipment, int consumedQuantity);

        public Dictionary<String, int> GetEquipmentPerQuantity();
    }
}
