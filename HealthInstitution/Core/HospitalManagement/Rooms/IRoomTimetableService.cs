using HealthInstitution.Core.Rooms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Rooms
{
    public interface IRoomTimetableService
    {
        public bool CheckRoomTimetable(Room selectedRoom, DateTime startDate, out string message);
    }
}
