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
        static EquipmentRepository s_equipmentRepository = EquipmentRepository.GetInstance();
        static RoomRepository s_roomRepository = RoomRepository.GetInstance();
        static EquipmentTransferRepository s_equipmentTransferRepository = EquipmentTransferRepository.GetInstance();
        public static void UpdateByTransfer()
        {
            List<int> equipmentTransfersToRemove = new List<int>();
            
            foreach (EquipmentTransfer equipmentTransfer in s_equipmentTransferRepository.EquipmentTransfers)
            {
                if (equipmentTransfer.TransferTime == DateTime.Today)
                {
                    Equipment equipmentFromRoom = equipmentTransfer.FromRoom.AvailableEquipment.Find(eq => (eq.Type == equipmentTransfer.Equipment.Type && eq.Name == equipmentTransfer.Equipment.Name));
                    Transfer(equipmentTransfer.ToRoom, equipmentFromRoom, equipmentTransfer.Equipment.Quantity);
                    equipmentTransfersToRemove.Add(equipmentTransfer.Id);
                }
            }
            RemoveOldTransfers(equipmentTransfersToRemove);
            
        }

        private static void RemoveOldTransfers(List<int> equipmentTransfersToRemove)
        {
            foreach (int id in equipmentTransfersToRemove)
            {
                s_equipmentTransferRepository.Delete(id);
            }
        }

        public static void Transfer(Room toRoom, Equipment equipment, int quantity)
        {
            equipment.Quantity -= quantity;
            int index = toRoom.AvailableEquipment.FindIndex(eq => (eq.Name == equipment.Name && eq.Type == equipment.Type));
            if (index >= 0)
            {
                toRoom.AvailableEquipment[index].Quantity += quantity;
                s_equipmentRepository.Save();
            }
            else
            {
                EquipmentDTO equipmentDTO = new EquipmentDTO(quantity, equipment.Name, equipment.Type, equipment.IsDynamic);
                Equipment newEquipment = s_equipmentRepository.Add(equipmentDTO);
                s_roomRepository.AddToRoom(toRoom.Id, newEquipment);
            }
        }

        public static void Transfer(List<Equipment> toRoomEquipments, Equipment equipment, int quantity)
        {
            equipment.Quantity -= quantity;
            int index = toRoomEquipments.FindIndex(eq => (eq.Name == equipment.Name && eq.Type == equipment.Type));
            if (index >= 0)
            {
                toRoomEquipments[index].Quantity += quantity;
                s_equipmentRepository.Save();
            }
            else
            {
                EquipmentDTO equipmentDTO = new EquipmentDTO(quantity, equipment.Name, equipment.Type, equipment.IsDynamic);
                Equipment newEquipment = s_equipmentRepository.Add(equipmentDTO);
                toRoomEquipments.Add(newEquipment);
            }
        }
    }
}
