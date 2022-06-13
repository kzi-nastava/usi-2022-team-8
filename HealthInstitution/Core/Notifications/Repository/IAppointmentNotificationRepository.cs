using HealthInstitution.Core.Notifications.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Notifications.Repository
{
    public interface IAppointmentNotificationRepository
    {
        public void LoadFromFile();
        public void Save();
        public List<AppointmentNotification> GetAll();
        public AppointmentNotification GetById(int id);
        public void Add(AppointmentNotificationDTO appointmentNotificationDTO);
        public void Delete(int id);
    }
}
