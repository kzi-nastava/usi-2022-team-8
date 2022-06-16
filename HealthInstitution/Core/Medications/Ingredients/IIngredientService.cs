using HealthInstitution.Core.Ingredients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Ingredients
{
    public interface IIngredientService
    {
        public bool CheckOccurrenceOfIngredient(Ingredient ingredient);
        public List<Ingredient> GetAll();
        public void Add(string name);
        public void Update(int id, string name);
        public void Delete(int id);
        public bool Contains(string name);
    }
}
