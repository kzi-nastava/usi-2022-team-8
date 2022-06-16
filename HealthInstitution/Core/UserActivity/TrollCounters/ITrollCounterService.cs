using HealthInstitution.Core.TrollCounters.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.TrollCounters
{
    public interface ITrollCounterService
    {
        public TrollCounter GetById(string id);
        public void Add(string username);
        public void Delete(string id);
        public void TrollCheck(string username);
        public void BlockPatient(string username);
        public void CheckCreateTroll(string username);
        public void CheckEditDeleteTroll(string username);
        public void AppendEditDeleteDates(string username);
        public void AppendCreateDates(string username);
    }
}
