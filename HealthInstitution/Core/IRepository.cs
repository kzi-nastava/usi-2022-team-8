using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core
{
    public interface IRepository<T> where T : class
    {
        public List<T> GetAll();
        public void Save();
        public void LoadFromFile();

    }
}
