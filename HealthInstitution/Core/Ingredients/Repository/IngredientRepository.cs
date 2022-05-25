using HealthInstitution.Core.Ingredients.Model;
using System.IO;
using System.Text.Json;

namespace HealthInstitution.Core.Ingredients.Repository;

public class IngredientRepository
{
    private int _maxId;
    private String _fileName;
    public List<Ingredient> Ingredients { get; set; }
    public Dictionary<int, Ingredient> IngredientById { get; set; }

    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    private IngredientRepository(string fileName) 
    {
        this._fileName = fileName;
        this.Ingredients = new List<Ingredient>();
        this.IngredientById = new Dictionary<int, Ingredient>();
        this._maxId = 0;
        this.LoadFromFile();
    }
    private static IngredientRepository s_instance = null;
    public static IngredientRepository GetInstance()
    {
        {
            if (s_instance == null)
            {
                s_instance = new IngredientRepository(@"..\..\..\Data\JSON\ingredients.json");
            }
            return s_instance;
        }
    }
    public void LoadFromFile()
    {
        var ingredients = JsonSerializer.Deserialize<List<Ingredient>>(File.ReadAllText(@"..\..\..\Data\JSON\ingredients.json"), _options);
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.Id > _maxId)
            {
                _maxId = ingredient.Id;
            }
            this.Ingredients.Add(ingredient);
            this.IngredientById[ingredient.Id] = ingredient;
        }
    }
    public void Save()
    {
        var allIngredients = JsonSerializer.Serialize(this.Ingredients, _options);
        File.WriteAllText(this._fileName, allIngredients);
    }

    public List<Ingredient> GetAll()
    {
        return this.Ingredients;
    }

    public Ingredient GetById(int id)
    {
        if (this.IngredientById.ContainsKey(id))
            return this.IngredientById[id];
        return null;
    }

    public void Add(string name)
    {
        this._maxId++;
        int id = this._maxId;
        Ingredient ingredient = new Ingredient(id, name);
        this.Ingredients.Add(ingredient);
        this.IngredientById[id] = ingredient;
        Save();
    }

    public void Update(int id, string name)
    {
        Ingredient ingredient = GetById(id);
        ingredient.Name = name;
        this.IngredientById[id] = ingredient;
        Save();
    }

    public void Delete(int id)
    {
        Ingredient ingredient = GetById(id);
        this.Ingredients.Remove(ingredient);
        this.IngredientById.Remove(id);
        Save();
    }

    public bool Contains(string name)
    {
        return this.Ingredients.Any(ingredient => ingredient.Name == name);
    }
    
}