using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Ingredients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.ViewModels.ModelViewModels.DrugVerification
{
    public class DrugViewModel : ViewModelBase
    {
        private Drug _drug;
        public string Name => _drug.Name;
        public List<Ingredient> Ingredients => _drug.Ingredients;
        public DrugViewModel(Drug drug)
        {
            _drug = drug;
        }
    }
}
