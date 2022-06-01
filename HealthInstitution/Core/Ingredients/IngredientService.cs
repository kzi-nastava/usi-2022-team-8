using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.Ingredients.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Ingredients
{
    public class IngredientService
    {
        IngredientRepository _ingredientRepository;

        public IngredientService()
        {
            _ingredientRepository = IngredientRepository.GetInstance();
        }

        public static bool CheckOccurrenceOfIngredient(Ingredient ingredient)
        {
            DrugRepository drugRepository = DrugRepository.GetInstance();
            return drugRepository.ContainsDrugWithIngredient(ingredient);
        }
    }
}
