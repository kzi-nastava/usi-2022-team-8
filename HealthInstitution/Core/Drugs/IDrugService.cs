using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Ingredients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Drugs
{
    public interface IDrugService
    {
        public List<Drug> GetAll();
        public Drug GetById(int id);
        public List<Drug> GetAllAccepted();
        public List<Drug> GetAllRejected();
        public List<Drug> GetAllCreated();
        public void Add(DrugDTO drugDTO);
        public void Update(int id, DrugDTO drugDTO);
        public void Delete(int id);
        public bool ContainsIngredient(Ingredient ingredient);


    }
}
