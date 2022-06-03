using HealthInstitution.Core.RecepieNotifications.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.RecepieNotifications.Repository
{
    public interface IRecepieNotificationSettingsRepository : IRepository<RecepieNotificationSettings>
    {
        public void LoadFromFile();
        public void Save();
        public RecepieNotificationSettings GetById(int id);
        public void Add(RecepieNotificationSettings recepieNotificationSettings);
        public void Delete(int id);
    }
}
