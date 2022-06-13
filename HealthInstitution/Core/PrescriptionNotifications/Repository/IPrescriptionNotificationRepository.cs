using HealthInstitution.Core.PrescriptionNotifications.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.PrescriptionNotifications.Repository
{
    public interface IPrescriptionNotificationRepository
    {
        public void LoadFromFile();
        public void Save();
        public void Add(PrescriptionNotification recepieNotification);
        public void Delete(int id);
        public void Update(int id, PrescriptionNotificationSettings settings);
        public List<PrescriptionNotification> GetPatientPresctiptionNotification(string username, int prescription);
        public List<PrescriptionNotification> GetPatientActiveNotification(string username);
    }
}