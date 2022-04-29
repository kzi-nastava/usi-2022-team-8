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
        public Room room { get; set; }
        public Equipment equipment { get; set; }

        public TableItemEquipment(Room room, Equipment equipment)
        {
            this.room = room;
            this.equipment = equipment;
        }
    }
}
