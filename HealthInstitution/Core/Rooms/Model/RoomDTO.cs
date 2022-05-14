using HealthInstitution.Core.Equipments.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Rooms.Model
{
    public class RoomDTO
    {
        public RoomType Type { get; set; }
        public int Number { get; set; }
        public bool IsRenovating { get; set; }
        public bool IsActive { get; set; }

        public RoomDTO(RoomType type, int number, bool isRenovating = false, bool isActive = true)
        {
            this.Type = type;
            this.Number = number;
            this.IsRenovating = isRenovating;
            this.IsActive = isActive;
        }
    }
}
