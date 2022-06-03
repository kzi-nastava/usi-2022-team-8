using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Examinations.Repository
{
    public interface IExaminationDoctorRepository
    {
        public ExaminationDoctorRepository GetInstance();
        public void LoadFromFile();
        public void Save();
    }
}
