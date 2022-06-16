using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.Ingredients.Repository;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.Drugs.Repository;

public class DrugRepository : IDrugRepository
{
    private int _maxId;
    private String _fileName = @"..\..\..\Data\drugs.json";
    private IIngredientRepository _ingredientRepository;
    public List<Drug> Drugs { get; set; }
    public Dictionary<int, Drug> DrugById { get; set; }

    public DrugRepository(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
        this.Drugs = new List<Drug>();
        this.DrugById = new Dictionary<int, Drug>();
        this._maxId = 0;
        this.LoadFromFile();
    }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };

    private List<Ingredient> JToken2Ingredients(JToken tokens)
    {
        Dictionary<int, Ingredient> ingredientById = _ingredientRepository.GetAllById();
        List<Ingredient> items = new List<Ingredient>();
        foreach (int token in tokens)
            items.Add(ingredientById[token]);
        return items;
    }

    private Drug Parse(JToken? drug)
    {
        DrugState drugState;
        Enum.TryParse<DrugState>((string)drug["state"], out drugState);
        return new Drug((int)drug["id"],
                                  (string)drug["name"],
                                  drugState,
                                  JToken2Ingredients(drug["ingredients"]),
                                  (string)drug["rejectionReason"]);
    }

    public void LoadFromFile()
    {
        var drugs = JArray.Parse(File.ReadAllText(_fileName));
        foreach (var drug in drugs)
        {
            Drug loadedDrug = Parse(drug);
            if (loadedDrug.Id > _maxId)
            {
                _maxId = loadedDrug.Id;
            }
            this.Drugs.Add(loadedDrug);
            this.DrugById[loadedDrug.Id] = loadedDrug;
        }
    }

    private List<dynamic> PrepareForSerialization()
    {
        List<dynamic> reducedDrugs = new List<dynamic>();
        foreach (var drug in this.Drugs)
        {
            List<int> ingredientsId = new List<int>();
            foreach (var i in drug.Ingredients)
                ingredientsId.Add(i.Id);
            reducedDrugs.Add(new
            {
                id = drug.Id,
                name = drug.Name,
                state = drug.State,
                ingredients = ingredientsId,
                rejectionReason = drug.RejectionReason
            });
        }
        return reducedDrugs;
    }

    public void Save()
    {
        var allDrugs = JsonSerializer.Serialize(PrepareForSerialization(), _options);
        File.WriteAllText(this._fileName, allDrugs);
    }

    public List<Drug> GetAll()
    {
        return this.Drugs;
    }

    public Dictionary<int, Drug> GetAllById()
    {
        return DrugById;
    }

    public List<Drug> GetAllAccepted()
    {
        return GetAllByStatus(DrugState.Accepted);
    }

    public List<Drug> GetAllCreated()
    {
        return GetAllByStatus(DrugState.Created);
    }

    public List<Drug> GetAllRejected()
    {
        return GetAllByStatus(DrugState.Rejected);
    }

    public List<Drug> GetAllByStatus(DrugState drugState)
    {
        List<Drug> rejectedDrugs = new List<Drug>();
        foreach (Drug drug in this.Drugs)
        {
            if (drug.State.Equals(drugState))
            {
                rejectedDrugs.Add(drug);
            }
        }
        return rejectedDrugs;
    }

    public Drug GetById(int id)
    {
        if (DrugById.ContainsKey(id))
            return DrugById[id];
        return null;
    }

    public void Add(Drug drug)
    {
        int id = ++_maxId;
        drug.Id = id;
        this.Drugs.Add(drug);
        this.DrugById[id] = drug;
        Save();
    }

    public void Update(int id, Drug byDrug)
    {
        Drug drug = GetById(id);
        drug.Name = byDrug.Name;
        drug.State = byDrug.State;
        drug.Ingredients = byDrug.Ingredients;
        drug.RejectionReason = byDrug.RejectionReason;
        DrugById[drug.Id] = drug;
        Save();
    }

    public void Accept(Drug drug)
    {
        drug.State = DrugState.Accepted;
        DrugById[drug.Id] = drug;
        Save();
    }

    public void Reject(Drug drug, string rejectionReason)
    {
        drug.State = DrugState.Rejected;
        drug.RejectionReason = rejectionReason;
        DrugById[drug.Id] = drug;
        Save();
    }

    public void Delete(int id)
    {
        Drug drug = GetById(id);
        this.Drugs.Remove(drug);
        this.DrugById.Remove(id);
        Save();
    }

    public bool ContainsIngredient(Ingredient ingredient)
    {
        foreach (Drug drug in this.Drugs)
        {
            if (drug.ContainsIngredient(ingredient))
            {
                return true;
            }
        }
        return false;
    }

    public bool Contains(string name)
    {
        return this.Drugs.Any(drug => drug.Name == name);
    }
}