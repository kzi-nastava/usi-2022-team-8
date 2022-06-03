using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HealthInstitution.Core.Equipments;

namespace HealthInstitution.Core.EquipmentTransfers
{
    internal static class EquipmentTransferService
    {
        private static EquipmentTransferRepository s_equipmentTransferRepository = EquipmentTransferRepository.GetInstance();

        public static List<EquipmentTransfer> GetAll()
        {
            return s_equipmentTransferRepository.GetAll();
        }

        public static void Add(EquipmentTransferDTO equipmentTransferDTO)
        {
            EquipmentTransfer equipmentTransfer = new EquipmentTransfer(equipmentTransferDTO);
            s_equipmentTransferRepository.Add(equipmentTransfer);
        }

        public static void Update(int id, EquipmentTransferDTO equipmentTransferDTO)
        {
            EquipmentTransfer equipmentTransfer = new EquipmentTransfer(equipmentTransferDTO);
            s_equipmentTransferRepository.Update(id, equipmentTransfer);
        }


        public static void Delete(int id)
        {
            s_equipmentTransferRepository.Delete(id);
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
        public static void ScheduleWarehouseRefill(string equipmentName, int quantity)
        {
            EquipmentType equipmentType = EquipmentRepository.GetInstance().GetEquipmentType(equipmentName);
            EquipmentDTO selectedEquipmentDTO = new EquipmentDTO(quantity, equipmentName, equipmentType, true);
            Equipment newEquipment = EquipmentService.Add(selectedEquipmentDTO);
            DateTime tomorrowSameTime = DateTime.Now + new TimeSpan(1, 0, 0, 0);
            EquipmentTransferDTO equipmentTransferDTO = new EquipmentTransferDTO(newEquipment, null, RoomRepository.GetInstance().RoomById[1], tomorrowSameTime);
            Add(equipmentTransferDTO);
        }
    }
}
