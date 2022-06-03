using HealthInstitution.Core.Operations.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Operations.Repository
{
    public interface IOperationRepository : IRepository<Operation>
    {
        public Operation Parse(JToken? operation);
        public void LoadFromFile();
        public List<dynamic> PrepareForSerialization();
        public void Save();
        public void ChangeStatus();
        public List<Operation> GetAll();
        public Operation GetById(int id);
        public void AddToCollections(Operation operation);
        public void Add(Operation operation);
        public void Update(int id, Operation byOperation);
        public void Delete(int id);
        public void SwapOperationValue(Operation operation);
        public List<Operation> GetByPatient(String username);
    }
}
