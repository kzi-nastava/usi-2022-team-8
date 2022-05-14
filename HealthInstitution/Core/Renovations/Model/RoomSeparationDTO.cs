using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Renovations.Model
{
    public class RoomSeparationDTO : RenovationDTO
    {
        public Room FirstRoom { get; set; }
        public Room SecondRoom { get; set; }

        public RoomSeparationDTO(Room initialRoom, Room firstRoom, Room secondRoom, DateTime startDate, DateTime endDate) : base(initialRoom, startDate, endDate)
        {
            this.FirstRoom = firstRoom;
            this.SecondRoom = secondRoom;
        }
    }
}
