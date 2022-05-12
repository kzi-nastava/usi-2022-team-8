using HealthInstitution.Core.Ingredients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Drugs.Model
{
    public class DrugDTO
    {
        public String Name { get; set; }
        public DrugState State { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public DrugDTO(string name, DrugState state, List<Ingredient> ingredients)
        {
            this.Name = name;
            this.State = state;
            this.Ingredients = ingredients;
        }
    }
}
