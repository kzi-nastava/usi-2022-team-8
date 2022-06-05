using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.Ingredients.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthInstitution.Core.Drugs;

namespace HealthInstitution.Core.Ingredients
{
    public class IngredientService : IIngredientService
    {
        IIngredientRepository _ingredientRepository;
        public IngredientService(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public bool CheckOccurrenceOfIngredient(Ingredient ingredient)
        {
            return DrugService.ContainsIngredient(ingredient);
        }
        
        public List<Ingredient> GetAll()
        {
            return _ingredientRepository.GetAll();
        }

        public void Add(string name)
        {
            _ingredientRepository.Add(name);
        }

        public void Update(int id, string name)
        {
            _ingredientRepository.Update(id, name);
        }

        public void Delete(int id)
        {
            _ingredientRepository.Delete(id);
        }

        public bool Contains(string name)
        {
            return _ingredientRepository.Contains(name);
        }
    }
}
