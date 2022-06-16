using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Drugs.Repository;
using HealthInstitution.Core.Ingredients.Model;

namespace HealthInstitution.Core.Drugs
{
    public class DrugService : IDrugService
    {
        IDrugRepository _drugRepository;
        public DrugService(IDrugRepository drugRepository)
        {
            _drugRepository = drugRepository;
        }
        public Drug GetById(int id)
        {
            return _drugRepository.GetById(id);
        }
        public List<Drug> GetAll()
        {
            return _drugRepository.GetAll();
        }
        public List<Drug> GetAllAccepted()
        {
            return _drugRepository.GetAllAccepted();
        }
        public List<Drug> GetAllCreated()
        {
            return _drugRepository.GetAllCreated();
        }
        public List<Drug> GetAllRejected()
        {
            return _drugRepository.GetAllRejected();
        }
        public void Add(DrugDTO drugDTO)
        {
            Drug drug = new Drug(drugDTO);
            _drugRepository.Add(drug);
        }
        public void Update(int id, DrugDTO drugDTO)
        {
            Drug drug = new Drug(drugDTO);
            _drugRepository.Update(id, drug);
        }
        public void Delete(int id)
        {
            _drugRepository.Delete(id);
        }
        public bool ContainsIngredient(Ingredient ingredient)
        {
            return _drugRepository.ContainsIngredient(ingredient);
        }

        public List<Ingredient> GetIngredients(Drug drug)
        {
            return drug.Ingredients;
        }

        public bool Contains(string name)
        {
            return _drugRepository.Contains(name);
        }
    }
}
