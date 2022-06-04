using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Ingredients.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthInstitution.Core.Drugs
{
    public static class DrugService
    {
        private static DrugRepository s_drugRepository = DrugRepository.GetInstance();
        public static Drug GetById(int id)
        {
            return s_drugRepository.GetById(id);
        }
        public static List<Drug> GetAll()
        {
            return s_drugRepository.GetAll();
        }
        public static List<Drug> GetAllAccepted()
        {
            return s_drugRepository.GetAllAccepted();
        }
        public static List<Drug> GetAllCreated()
        {
            return s_drugRepository.GetAllCreated();
        }
        public static List<Drug> GetAllRejected()
        {
            return s_drugRepository.GetAllRejected();
        }
        public static void Add(DrugDTO drugDTO)
        {
            Drug drug = new Drug(drugDTO);
            s_drugRepository.Add(drug);
        }
        public static void Update(int id, DrugDTO drugDTO)
        {
            Drug drug = new Drug(drugDTO);
            s_drugRepository.Update(id, drug);
        }
        public static void Delete(int id)
        {
            s_drugRepository.Delete(id);
        }
        public static bool ContainsIngredient(Ingredient ingredient)
        {
            return s_drugRepository.ContainsIngredient(ingredient);
        }

        public static List<Ingredient> GetIngredients(Drug drug)
        {
            return drug.Ingredients;
        }

        public static bool Contains(string name)
        {
            return s_drugRepository.Contains(name);
        }
    }
}
