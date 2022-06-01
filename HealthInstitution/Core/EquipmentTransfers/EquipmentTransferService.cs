using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HealthInstitution.Core.EquipmentTransfers
{
    internal class EquipmentTransferService
    {
        private EquipmentTransferService()
        {
        }
        public static void ScheduleWarehouseRefill(string equipmentName, int quantity)
        {
            EquipmentType equipmentType = EquipmentRepository.GetInstance().GetEquipmentType(equipmentName);
            EquipmentDTO selectedEquipmentDTO = new EquipmentDTO(quantity, equipmentName, equipmentType, true);
            Equipment newEquipment = EquipmentRepository.GetInstance().Add(selectedEquipmentDTO);
            DateTime tomorrowSameTime = DateTime.Now + new TimeSpan(1, 0, 0, 0);
            EquipmentTransferDTO equipmentTransferDTO = new EquipmentTransferDTO(newEquipment, null, RoomRepository.GetInstance().RoomById[1], tomorrowSameTime);
            EquipmentTransferRepository.GetInstance().Add(equipmentTransferDTO);
        }
    }
}
