using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.EquipmentTransfers
{
    public interface IEquipmentTransferService
    {
        public List<EquipmentTransfer> GetAll();
        public void Add(EquipmentTransferDTO equipmentTransferDTO);
        public void Update(int id, EquipmentTransferDTO equipmentTransferDTO);
        public void Delete(int id);
        public bool CheckOccurrenceOfRoom(Room room);
        public void Transfer(Room toRoom, Equipment equipment, int quantity);
        public void Transfer(List<Equipment> toRoomEquipments, Equipment equipment, int quantity);
        public int CalculateProjectedQuantityLoss(Room fromRoom, Equipment equipment);
        public void ScheduleWarehouseRefill(string equipmentName, int quantity);
    }
}
