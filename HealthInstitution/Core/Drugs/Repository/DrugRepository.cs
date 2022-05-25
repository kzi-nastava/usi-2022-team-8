using HealthInstitution.Core.Drugs.Model;
using HealthInstitution.Core.Ingredients.Model;
using HealthInstitution.Core.Ingredients.Repository;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthInstitution.Core.Drugs.Repository;

public class DrugRepository
{
    private int _maxId;
    private String _fileName;
    public List<Drug> Drugs { get; set; }
    public Dictionary<int, Drug> DrugById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true
    };
    private DrugRepository(string fileName) //singleton
    {
        this._fileName = fileName;
        this.Drugs = new List<Drug>();
        this.DrugById = new Dictionary<int, Drug>();
        this._maxId = 0;
        this.LoadFromFile();
    }
    private static DrugRepository s_instance = null;
    public static DrugRepository GetInstance()
    {
        {
            if (s_instance == null)
            {
                s_instance = new DrugRepository(@"..\..\..\Data\JSON\drugs.json");
            }
            return s_instance;
        }
    }
    private List<Ingredient> JToken2Ingredients(JToken tokens)
    {
        Dictionary<int, Ingredient> ingredientById = IngredientRepository.GetInstance().IngredientById;
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
                                  JToken2Ingredients(drug["ingredients"]));
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
                id=drug.Id,
                name=drug.Name,
                state=drug.State,
                ingredients=ingredientsId
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

    public Drug GetById(int id)
    {
        if (DrugById.ContainsKey(id))
            return DrugById[id];
        return null;
    }

    public void Add(DrugDTO drugDTO)
    {
        this._maxId++;
        int id = this._maxId;
        Drug drug = new Drug(id, drugDTO.Name, drugDTO.State, drugDTO.Ingredients);
        this.Drugs.Add(drug);
        this.DrugById[id] = drug;
        Save();
    }

    public void Update(int id, DrugDTO drugDTO)
    {
        Drug drug = GetById(id);
        drug.Name = drugDTO.Name;
        drug.State = drugDTO.State;
        drug.Ingredients= drugDTO.Ingredients;
        DrugById[id] = drug;
        Save();
    }

    public void Delete(int id)
    {
        Drug drug = GetById(id);
        this.Drugs.Remove(drug);
        this.DrugById.Remove(id);
        Save();
    }

    public bool ContainsDrugWithIngredient(Ingredient ingredient)
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
}