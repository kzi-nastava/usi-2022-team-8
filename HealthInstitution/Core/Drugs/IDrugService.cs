using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Ingredients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Drugs
{
    public interface IDrugService : IService<Drug> 
    {
        public Task<IEnumerable<Drug>> GetAll();
        public Task<Drug> GetById(int id);
        public Task<IEnumerable<Drug>> GetAllAccepted();
        public Task<IEnumerable<Drug>> GetAllRejected();
        public Task<IEnumerable<Drug>> GetAllCreated();
        public void Add(DrugDTO drugDTO);
        public void Update(int id, DrugDTO drugDTO);
        public void Delete(int id);
        public void Accept(Drug drug);
        public void Reject(Drug drug, string rejectionReason);
        public bool ContainsIngredient(Ingredient ingredient);


    }
}
