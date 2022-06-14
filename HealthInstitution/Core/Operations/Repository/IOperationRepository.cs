using HealthInstitution.Core.Operations.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Operations.Repository
{
    public interface IOperationRepository
    {
        public void LoadFromFile();
        public void Save();
        public List<Operation> GetAll();
        public Dictionary<int, Operation> GetAllById();
        public Operation GetById(int id);
        public void Add(Operation operation);
        public void Update(int id, Operation byOperation);
        public void Delete(int id);
        public void SwapOperationValue(Operation operation);
        public List<Operation> GetByPatient(String username);
        public List<Operation> GetByDoctor(String username);
    }
}
