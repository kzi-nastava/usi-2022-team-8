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
    public class EquipmentTransferService : IEquipmentTransferService
    {
        IEquipmentRepository _equipmentRepository;
        IEquipmentTransferRepository _equipmentTransferRepository;
        IRoomService _roomService;
        IEquipmentService _equipmentService;
        public EquipmentTransferService(IEquipmentRepository equipmentRepository, 
            IEquipmentTransferRepository equipmentTransferRepository, IRoomService roomService,
        IEquipmentService equipmentService
            )
        {
            _equipmentService = equipmentService;
            _roomService = roomService;
            _equipmentRepository = equipmentRepository;
            _equipmentTransferRepository = equipmentTransferRepository;
        }
        public List<EquipmentTransfer> GetAll()
        {
            return _equipmentTransferRepository.GetAll();
        }

        public void Add(EquipmentTransferDTO equipmentTransferDTO)
        {
            EquipmentTransfer equipmentTransfer = new EquipmentTransfer(equipmentTransferDTO);
            _equipmentTransferRepository.Add(equipmentTransfer);
        }

        public void Update(int id, EquipmentTransferDTO equipmentTransferDTO)
        {
            EquipmentTransfer equipmentTransfer = new EquipmentTransfer(equipmentTransferDTO);
            _equipmentTransferRepository.Update(id, equipmentTransfer);
        }


        public void Delete(int id)
        {
            _equipmentTransferRepository.Delete(id);
        }
        public bool CheckOccurrenceOfRoom(Room room)
        {
            if (_equipmentTransferRepository.GetAll().Find(eqTransfer => eqTransfer.FromRoom == room || eqTransfer.ToRoom == room) == null)
            {
                return false;
            }
            return true;
        }

        public void Transfer(Room toRoom, Equipment equipment, int quantity)
        {
            equipment.Quantity -= quantity;
            int index = toRoom.AvailableEquipment.FindIndex(eq => (eq.Name == equipment.Name && eq.Type == equipment.Type));
            if (index >= 0)
            {
                toRoom.AvailableEquipment[index].Quantity += quantity;
                _equipmentRepository.Save();
            }
            else
            {
                EquipmentDTO equipmentDTO = new EquipmentDTO(quantity, equipment.Name, equipment.Type, equipment.IsDynamic);
                Equipment newEquipment = (toRoom.IsWarehouse()) ? equipment : _equipmentService.Add(equipmentDTO);
                _roomService.AddToRoom(toRoom.Id, newEquipment);
            }
        }

        public void Transfer(List<Equipment> toRoomEquipments, Equipment equipment, int quantity)
        {
            equipment.Quantity -= quantity;
            int index = toRoomEquipments.FindIndex(eq => (eq.Name == equipment.Name && eq.Type == equipment.Type));
            if (index >= 0)
            {
                toRoomEquipments[index].Quantity += quantity;
                _equipmentRepository.Save();
            }
            else
            {
                EquipmentDTO equipmentDTO = new EquipmentDTO(quantity, equipment.Name, equipment.Type, equipment.IsDynamic);
                Equipment newEquipment = _equipmentService.Add(equipmentDTO);
                toRoomEquipments.Add(newEquipment);
            }
        }

        public int CalculateProjectedQuantityLoss(Room fromRoom, Equipment equipment)
        {
            int projectedQuantityLoss = 0;
           
            foreach (EquipmentTransfer equipmentTransfer in _equipmentTransferRepository.GetAll())
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
        public void ScheduleWarehouseRefill(string equipmentName, int quantity)
        {
            EquipmentType equipmentType = _equipmentRepository.GetEquipmentType(equipmentName);
            EquipmentDTO selectedEquipmentDTO = new EquipmentDTO(quantity, equipmentName, equipmentType, true);
            Equipment newEquipment = _equipmentService.Add(selectedEquipmentDTO);
            DateTime tomorrowSameTime = DateTime.Now + new TimeSpan(1, 0, 0, 0);
            EquipmentTransferDTO equipmentTransferDTO = new EquipmentTransferDTO(newEquipment, null, RoomRepository.GetInstance().RoomById[1], tomorrowSameTime);
            Add(equipmentTransferDTO);
        }
    }
}
