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
    public class EquipmentTransferRefreshingService : IEquipmentTransferRefreshingService
    {
        IEquipmentRepository _equipmentRepository;
        IEquipmentTransferRepository _equipmentTransferRepository;
        IEquipmentTransferService _equipmentTransferService;
        public EquipmentTransferRefreshingService(IEquipmentRepository equipmentRepository, IEquipmentTransferRepository equipmentTransferRepository, IEquipmentTransferService equipmentTransferService)
        {
            _equipmentRepository = equipmentRepository;
            _equipmentTransferRepository = equipmentTransferRepository;
            _equipmentTransferService = equipmentTransferService;
        }
        public  void UpdateByTransfer()
        {
            List<int> equipmentTransfersToRemove = new List<int>();
            
            foreach (EquipmentTransfer equipmentTransfer in _equipmentTransferRepository.GetAll())
            {
                if(equipmentTransfer.ToRoom.IsWarehouse() && equipmentTransfer.TransferTime<=DateTime.Now)
                {
                    FillWarehouse(equipmentTransfer, equipmentTransfersToRemove);
                }
                else if (equipmentTransfer.TransferTime <= DateTime.Today)
                {
                    Equipment equipmentFromRoom = equipmentTransfer.FromRoom.AvailableEquipment.Find(eq => (eq.Type == equipmentTransfer.Equipment.Type && eq.Name == equipmentTransfer.Equipment.Name));
                    _equipmentTransferService.Transfer(equipmentTransfer.ToRoom, equipmentFromRoom, equipmentTransfer.Equipment.Quantity);
                    equipmentTransfersToRemove.Add(equipmentTransfer.Id);
                }
            }
            RemoveOldTransfers(equipmentTransfersToRemove);
            
        }
        private void FillWarehouse(EquipmentTransfer equipmentTransfer, List<int> equipmentTransfersToRemove)
        {
            Equipment purchasedEquipment = _equipmentRepository.GetById(equipmentTransfer.Equipment.Id);
            _equipmentTransferService.Transfer(equipmentTransfer.ToRoom, purchasedEquipment, purchasedEquipment.Quantity);
            equipmentTransfersToRemove.Add(equipmentTransfer.Id);
        }

        private void RemoveOldTransfers(List<int> equipmentTransfersToRemove)
        {
            foreach (int id in equipmentTransfersToRemove)
            {
                _equipmentTransferService.Delete(id);
            }
        }
    }
}
