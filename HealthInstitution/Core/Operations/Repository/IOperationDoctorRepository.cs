using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Operations.Repository
{
    public interface IOperationDoctorRepository : ILinkerRepository
    {
        public void LoadFromFile();
        public void Save();
    }
}
