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
        Converters = { new JsonStringEnumConverter() }
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
    private List<Ingredient> jToken2Ingredients(JToken tokens)
    {
        Dictionary<int, Ingredient> ingredientById = IngredientRepository.GetInstance().IngredientById;
        List<Ingredient> items = new List<Ingredient>();
        foreach (int token in tokens)
            items.Add(ingredientById[token]);
        return items;
    }
    public void LoadFromFile()
    {
        var drugs = JArray.Parse(File.ReadAllText(_fileName));foreach (var drug in drugs)
        {
            DrugState drugState;
            Enum.TryParse<DrugState>((string)drug["state"], out drugState);
            Drug drugTemp = new Drug((int)drug["id"],
                                      (string)drug["name"],
                                      drugState,
                                      jToken2Ingredients(drug["ingredients"]));
            if (drugTemp.Id > _maxId)
            {
                _maxId = drugTemp.Id;
            }
            this.Drugs.Add(drugTemp);
            this.DrugById[drugTemp.Id] = drugTemp;
        }
    }
    private List<dynamic> shortenDrug()
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
        var allDrugs = JsonSerializer.Serialize(shortenDrug(), _options);
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

    public void Add(string name, DrugState drugState, List<Ingredient> ingredients)
    {
        this._maxId++;
        int id = this._maxId;
        Drug drug = new Drug(id, name, drugState, ingredients);
        this.Drugs.Add(drug);
        this.DrugById[id] = drug;
        Save();
    }

    public void Update(int id, string name, DrugState drugState, List<Ingredient> ingredients)
    {
        Drug drug = GetById(id);
        drug.Name = name;
        drug.State = drugState;
        drug.Ingredients= ingredients;
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
}