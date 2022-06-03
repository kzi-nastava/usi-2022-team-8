using HealthInstitution.Core.PrescriptionNotifications.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.PrescriptionNotifications.Repository
{
    public interface IRecepieNotificationRepository : IRepository<RecepieNotification>
    {
        public void LoadFromFile();
        public void Save();
        public void Add(RecepieNotification recepieNotification);
        public void Delete(int id);
        public List<RecepieNotification> GetPatientPresctiptionNotification(string username, int prescription);
        public List<RecepieNotification> GetPatientActiveNotification(string username);
    }
}
