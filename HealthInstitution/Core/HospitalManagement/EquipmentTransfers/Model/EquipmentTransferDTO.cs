using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.EquipmentTransfers.Model
{
    public class EquipmentTransferDTO
    {
        public Equipment Equipment { get; set; }
        public Room? FromRoom { get; set; }
        public Room ToRoom { get; set; }
        public DateTime TransferTime { get; set; }

        public EquipmentTransferDTO(Equipment equipment, Room? fromRoom, Room toRoom, DateTime transferTime)
        {
            this.Equipment = equipment;
            this.FromRoom = fromRoom;
            this.ToRoom = toRoom;
            this.TransferTime = transferTime;
        }
    }
}
