using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Equipments.Repository;
using HealthInstitution.Core.EquipmentTransfers.Model;
using HealthInstitution.Core.EquipmentTransfers.Repository;
using HealthInstitution.Core.Rooms.Repository;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HealthInstitution.Core.Equipments;
using HealthInstitution.Core.Rooms;

namespace HealthInstitution.Core.EquipmentTransfers
{
    public static class EquipmentTransferService
    {
        private static EquipmentTransferRepository s_equipmentTransferRepository = EquipmentTransferRepository.GetInstance();
        private static EquipmentRepository s_equipmentRepository = EquipmentRepository.GetInstance();
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
            if (s_equipmentTransferRepository.EquipmentTransfers.Find(eqTransfer => eqTransfer.FromRoom == room || eqTransfer.ToRoom == room) == null)
            {
                return false;
            }
            return true;
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
                Equipment newEquipment = (toRoom.IsWarehouse()) ? equipment : EquipmentService.Add(equipmentDTO);
                RoomService.AddToRoom(toRoom.Id, newEquipment);
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
                Equipment newEquipment = EquipmentService.Add(equipmentDTO);
                toRoomEquipments.Add(newEquipment);
            }
        }

        public static int CalculateProjectedQuantityLoss(Room fromRoom, Equipment equipment)
        {
            int projectedQuantityLoss = 0;
           
            foreach (EquipmentTransfer equipmentTransfer in s_equipmentTransferRepository.GetAll())
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
            EquipmentType equipmentType = s_equipmentRepository.GetEquipmentType(equipmentName);
            EquipmentDTO selectedEquipmentDTO = new EquipmentDTO(quantity, equipmentName, equipmentType, true);
            Equipment newEquipment = EquipmentService.Add(selectedEquipmentDTO);
            DateTime tomorrowSameTime = DateTime.Now + new TimeSpan(1, 0, 0, 0);
            EquipmentTransferDTO equipmentTransferDTO = new EquipmentTransferDTO(newEquipment, null, RoomRepository.GetInstance().RoomById[1], tomorrowSameTime);
            Add(equipmentTransferDTO);
        }
    }
}
