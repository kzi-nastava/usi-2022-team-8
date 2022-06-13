using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.EquipmentTransfers.Functionality
{
    public static class EquipmentTransferRefreshingService
    {
        private static EquipmentRepository s_equipmentRepository = EquipmentRepository.GetInstance();
        private static RoomRepository s_roomRepository = RoomRepository.GetInstance();
        private static EquipmentTransferRepository s_equipmentTransferRepository = EquipmentTransferRepository.GetInstance();
        public static void UpdateByTransfer()
        {
            List<int> equipmentTransfersToRemove = new List<int>();
            
            foreach (EquipmentTransfer equipmentTransfer in EquipmentTransferRepository.GetInstance().GetAll())
            {
                if(equipmentTransfer.ToRoom.IsWarehouse() && equipmentTransfer.TransferTime<=DateTime.Now)
                {
                    FillWarehouse(equipmentTransfer, equipmentTransfersToRemove);
                }
                else if (equipmentTransfer.TransferTime <= DateTime.Today)
                {
                    Equipment equipmentFromRoom = equipmentTransfer.FromRoom.AvailableEquipment.Find(eq => (eq.Type == equipmentTransfer.Equipment.Type && eq.Name == equipmentTransfer.Equipment.Name));
                    EquipmentTransferService.Transfer(equipmentTransfer.ToRoom, equipmentFromRoom, equipmentTransfer.Equipment.Quantity);
                    equipmentTransfersToRemove.Add(equipmentTransfer.Id);
                }
            }
            RemoveOldTransfers(equipmentTransfersToRemove);
            
        }
        private static void FillWarehouse(EquipmentTransfer equipmentTransfer, List<int> equipmentTransfersToRemove)
        {
            Equipment purchasedEquipment = s_equipmentRepository.EquipmentById[equipmentTransfer.Equipment.Id];
            EquipmentTransferService.Transfer(equipmentTransfer.ToRoom, purchasedEquipment, purchasedEquipment.Quantity);
            equipmentTransfersToRemove.Add(equipmentTransfer.Id);
        }

        private static void RemoveOldTransfers(List<int> equipmentTransfersToRemove)
        {
            foreach (int id in equipmentTransfersToRemove)
            {
                EquipmentTransferService.Delete(id);
            }
        }

        
    }
}
