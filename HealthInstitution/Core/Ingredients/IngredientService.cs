using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.Ingredients.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Ingredients
{
    static class IngredientService
    {
        static IngredientRepository s_ingredientRepository = IngredientRepository.GetInstance();
        public static List<Ingredient> GetAll()
        {
            return s_ingredientRepository.GetAll();
        }

        public static void Add(string name)
        {
            s_ingredientRepository.Add(name);
        }

        public static void Update(int id, string name)
        {
            s_ingredientRepository.Update(id, name);
        }

        public static void Delete(int id)
        {
            s_ingredientRepository.Delete(id);
        }
    }
}
