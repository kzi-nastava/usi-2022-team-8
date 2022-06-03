using HealthInstitution.Core.Equipments.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Equipments.Repository
{
    public interface IEquipmentRepository : IRepository<Equipment>
    {
        public EquipmentRepository GetInstance();

        public void LoadFromFile();

        public void FillTotalQuantityOfEquipment();

        public void Save();

        public List<Equipment> GetAll();

        public Equipment GetById(int id);

        public Equipment Add(Equipment equipment);

        public void Update(int id, Equipment byEquipment);
        public void Delete(int id);
        public EquipmentType GetEquipmentType(string equipmentName);
        public void RemoveConsumed(Equipment equipment, int consumedQuantity);
    }
}
