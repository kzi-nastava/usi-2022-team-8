using HealthInstitution.Core.TrollCounters.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.TrollCounters.Repository
{
    public interface ITrollCounterFileRepository
    {
        public void LoadFromFile();
        public void Save();
        public TrollCounter GetById(string id);
        public void Add(TrollCounter trollCounter);
        public void Delete(TrollCounter trollCounter);
        public void CheckCreateTroll(string username);
        public void CheckEditDeleteTroll(string username);
        public void AppendEditDeleteDates(string username);
        public void AppendCreateDates(string username);
    }
}
