using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.EquipmentTransfers
{
    public class EquipmentTransferService
    {
        private EquipmentTransferRepository _equipmentTransferRepository;

        public EquipmentTransferService()
        {
            _equipmentTransferRepository = EquipmentTransferRepository.GetInstance();
        }

        public static bool CheckOccurrenceOfRoom(Room room)
        {
            EquipmentTransferRepository equipmentTransferRepository = EquipmentTransferRepository.GetInstance();
            if (equipmentTransferRepository.EquipmentTransfers.Find(eqTransfer => eqTransfer.FromRoom == room || eqTransfer.ToRoom == room) == null)
            {
                return false;
            }
            return true;
        }

        public static int CalculateProjectedQuantityLoss(Room fromRoom, Equipment equipment)
        {
            int projectedQuantityLoss = 0;
            EquipmentTransferRepository equipmentTransferRepository = EquipmentTransferRepository.GetInstance();

            foreach (EquipmentTransfer equipmentTransfer in equipmentTransferRepository.GetAll())
            {
                if (equipmentTransfer.FromRoom == fromRoom)
                {
                    if (equipmentTransfer.Equipment.Name == equipment.Name && equipmentTransfer.Equipment.Type == equipment.Type)
                    {
                        projectedQuantityLoss += equipmentTransfer.Equipment.Quantity;
                    }
                }
            }
            return projectedQuantityLoss;
        }
    }
}
