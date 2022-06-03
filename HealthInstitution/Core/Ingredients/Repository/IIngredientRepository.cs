using HealthInstitution.Core.Examinations.Model;
using HealthInstitution.Core.Ingredients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Ingredients.Repository
{
    public interface IIngredientRepository : IRepository<Ingredient>
    {
        public void LoadFromFile();
        public void Save();
        public List<Ingredient> GetAll();
        public Ingredient GetById(int id);
        public void Add(string name);
        public void Update(int id, string name);
        public void Delete(int id);
        public bool Contains(string name);
    }
}
