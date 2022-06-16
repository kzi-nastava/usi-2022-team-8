using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Renovations.Model
{
    public class RoomMergerDTO : RenovationDTO
    {
        public Room RoomForMerge { get; set; }
        public Room MergedRoom { get; set; }
        public RoomMergerDTO(Room initialRoom, Room roomForMerge, Room mergedRoom, DateTime startDate, DateTime endDate) : base(initialRoom, startDate, endDate)
        {
            this.RoomForMerge = roomForMerge;
            this.MergedRoom = mergedRoom;
        }

    }
}
