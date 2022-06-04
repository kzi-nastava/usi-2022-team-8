using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core
{
    public interface ILinkerRepository
    {
        public void Save();
        public void LoadFromFile();
    }
}
