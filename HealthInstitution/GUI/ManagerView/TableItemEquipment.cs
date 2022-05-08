using HealthInstitution.Core.Equipments.Model;
using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.GUI.ManagerView
{
    public class TableItemEquipment
    {
        public Room Room { get; set; }
        public Equipment Equipment { get; set; }

        public TableItemEquipment(Room room, Equipment equipment)
        {
            this.Room = room;
            this.Equipment = equipment;
        }
    }
}
