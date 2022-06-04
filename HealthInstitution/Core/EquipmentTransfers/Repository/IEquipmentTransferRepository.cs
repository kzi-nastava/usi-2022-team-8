using HealthInstitution.Core.EquipmentTransfers.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.EquipmentTransfers.Repository
{
    public interface IEquipmentTransferRepository : IRepository<EquipmentTransfer>
    {
        public void LoadFromFile();
        public void Save();
        public List<EquipmentTransfer> GetAll();
        public EquipmentTransfer GetById(int id);
        public void Add(EquipmentTransfer equipmentTransfer);
        public void Update(int id, EquipmentTransfer byEquipmentTransfer);
        public void Delete(int id);
    }
}
