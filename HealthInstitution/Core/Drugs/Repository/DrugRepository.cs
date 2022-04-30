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
    private int maxId;
    public string fileName { get; set; }
    public List<Drug> drugs { get; set; }
    public Dictionary<int, Drug> drugById { get; set; }

    JsonSerializerOptions options = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() }
    };
    private DrugRepository(string fileName) //singleton
    {
        this.fileName = fileName;
        this.drugs = new List<Drug>();
        this.drugById = new Dictionary<int, Drug>();
        this.maxId = 0;
        this.LoadDrugs();
    }
    private static DrugRepository instance = null;
    public static DrugRepository GetInstance()
    {
        {
            if (instance == null)
            {
                instance = new DrugRepository(@"..\..\..\Data\JSON\drugs.json");
            }
            return instance;
        }
    }
    public List<Ingredient> JToken2Ingredients(JToken tokens)
    {
        Dictionary<int, Ingredient> ingredientById = IngredientRepository.GetInstance().ingredientById;
        List<Ingredient> items = new List<Ingredient>();
        foreach (int token in tokens)
            items.Add(ingredientById[token]);
        return items;
    }
    public void LoadDrugs()
    {
        var drugs = JArray.Parse(File.ReadAllText(fileName));
        //var medicalRecords = JsonSerializer.Deserialize<List<MedicalRecord>>(File.ReadAllText(@"..\..\..\Data\JSON\medicalRecords.json"), options);
        foreach (var drug in drugs)
        {
            DrugState drugState;
            Enum.TryParse<DrugState>((string)drug["state"], out drugState);
            Drug drugTemp = new Drug((int)drug["id"],
                                      (string)drug["name"],
                                      drugState,
                                      JToken2Ingredients(drug["ingredients"]));
            if (drugTemp.id > maxId)
            {
                maxId = drugTemp.id;
            }
            this.drugs.Add(drugTemp);
            this.drugById[drugTemp.id] = drugTemp;
        }
    }
    public List<dynamic> ShortenDrug()
    {
        List<dynamic> reducedDrugs = new List<dynamic>();
        foreach (var drug in this.drugs)
        {
            List<int> ingredientsId = new List<int>();
            foreach (var i in drug.ingredients)
                ingredientsId.Add(i.id);
            reducedDrugs.Add(new
            {
                id=drug.id,
                name=drug.name,
                state=drug.state,
                ingredients=ingredientsId
            });
        }
        return reducedDrugs;
    }
    public void SaveMedicalRecords()
    {

        var allMedicalRecords = JsonSerializer.Serialize(ShortenDrug(), options);
        File.WriteAllText(this.fileName, allMedicalRecords);
    }

    public List<Drug> GetDrugs()
    {
        return this.drugs;
    }

    public Drug GetDrugById(int id)
    {
        if (drugById.ContainsKey(id))
            return drugById[id];
        return null;
    }

    public void AddMedicalRecord(string name, DrugState drugState, List<Ingredient> ingredients)
    {
        this.maxId++;
        int id = this.maxId;
        Drug drug = new Drug(id, name, drugState, ingredients);
        this.drugs.Add(drug);
        this.drugById[id] = drug;
        SaveMedicalRecords();
    }

    public void UpdateMedicalRecord(int id, string name, DrugState drugState, List<Ingredient> ingredients)
    {
        Drug drug = GetDrugById(id);
        drug.name = name;
        drug.state = drugState;
        drug.ingredients= ingredients;
        drugById[id] = drug;
        SaveMedicalRecords();
    }

    public void DeleteDrug(int id)
    {
        Drug drug = GetDrugById(id);
        this.drugs.Remove(drug);
        this.drugById.Remove(id);
        SaveMedicalRecords();
    }
}