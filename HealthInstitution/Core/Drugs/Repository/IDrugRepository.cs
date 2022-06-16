using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Ingredients.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Drugs.Repository
{
    public interface IDrugRepository
    {
        public void LoadFromFile();
        public void Save();
        public List<Drug> GetAll();
        public List<Drug> GetAllAccepted();
        public List<Drug> GetAllCreated();
        public List<Drug> GetAllRejected();
        public List<Drug> GetAllByStatus(DrugState drugState);
        public Drug GetById(int id);
        public void Add(Drug drug);
        public void Update(int id, Drug byDrug);
        public void Accept(Drug drug);
        public void Reject(Drug drug, string rejectionReason);
        public void Delete(int id);
        public bool ContainsIngredient(Ingredient ingredient);
        public bool Contains(string name);
        public Dictionary<int, Drug> GetAllById();
    }
}
