using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms.Model;
using HealthInstitution.Core.Rooms.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.EquipmentTransfers.Functionality
{
    public class EquipmentTransferChecker
    {
        static EquipmentRepository equipmentRepository = EquipmentRepository.GetInstance();
        static RoomRepository roomRepository = RoomRepository.GetInstance();
        static EquipmentTransferRepository equipmentTransferRepository = EquipmentTransferRepository.GetInstance();
        public static void UpdateEquipmentByTransfer()
        {
            List<int> equipmentTransfersToRemove = new List<int>();
            foreach (EquipmentTransfer equipmentTransfer in equipmentTransferRepository.equipmentTransfers)
            {
                if (equipmentTransfer.transferTime == DateTime.Today)
                {
                    Equipment equipmentFromRoom = equipmentTransfer.fromRoom.availableEquipment.Find(eq => (eq.type == equipmentTransfer.equipment.type && eq.name == equipmentTransfer.equipment.name));
                    Transfer(equipmentTransfer.toRoom, equipmentFromRoom, equipmentTransfer.equipment.quantity);
                    equipmentTransfersToRemove.Add(equipmentTransfer.id);
                }
            }

            foreach (int id in equipmentTransfersToRemove)
            {
                equipmentTransferRepository.DeleteEquipmentTransfer(id);
            }
        }

        public static void Transfer(Room toRoom, Equipment equipment, int quantity)
        {
            equipment.quantity -= quantity;
            int index = toRoom.availableEquipment.FindIndex(eq => (eq.name == equipment.name && eq.type == equipment.type));
            if (index >= 0)
            {
                toRoom.availableEquipment[index].quantity += quantity;
                equipmentRepository.SaveEquipments();
            }
            else
            {
                Equipment newEquipment = equipmentRepository.AddEquipment(quantity, equipment.name, equipment.type, equipment.isDynamic);
                roomRepository.AddEquipmentToRoom(toRoom.id, newEquipment);
            }
        }
    }
}
