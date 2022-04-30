using HealthInstitution.Core.Ingredients.Model;
using System.IO;
using System.Text.Json;

namespace HealthInstitution.Core.Ingredients.Repository;

public class IngredientRepository
{
    private int maxId;
    public string fileName { get; set; }
    public List<Ingredient> ingredients { get; set; }
    public Dictionary<int, Ingredient> ingredientById { get; set; }

    private IngredientRepository(string fileName) 
    {
        this.fileName = fileName;
        this.ingredients = new List<Ingredient>();
        this.ingredientById = new Dictionary<int, Ingredient>();
        this.maxId = 0;
        this.LoadIngredients();
    }
    private static IngredientRepository instance = null;
    public static IngredientRepository GetInstance()
    {
        {
            if (instance == null)
            {
                instance = new IngredientRepository(@"..\..\..\Data\JSON\ingredients.json");
            }
            return instance;
        }
    }
    public void LoadIngredients()
    {
        var ingredients = JsonSerializer.Deserialize<List<Ingredient>>(File.ReadAllText(@"..\..\..\Data\JSON\ingredients.json"));
        foreach (Ingredient ingredient in ingredients)
        {
            if (ingredient.id > maxId)
            {
                maxId = ingredient.id;
            }
            this.ingredients.Add(ingredient);
            this.ingredientById[ingredient.id] = ingredient;
        }
    }
    public void SaveIngredients()
    {
        var allIngredients = JsonSerializer.Serialize(this.ingredients);
        File.WriteAllText(this.fileName, allIngredients);
    }

    public List<Ingredient> GetIngredients()
    {
        return this.ingredients;
    }

    public Ingredient GetIngredientById(int id)
    {
        if (this.ingredientById.ContainsKey(id))
            return this.ingredientById[id];
        return null;
    }

    public void AddIngredient(string name)
    {
        this.maxId++;
        int id = this.maxId;
        Ingredient ingredient = new Ingredient(id, name);
        this.ingredients.Add(ingredient);
        this.ingredientById[id] = ingredient;
        SaveIngredients();
    }

    public void UpdateIngredient(int id, string name)
    {
        Ingredient ingredient = GetIngredientById(id);
        ingredient.name = name;
        this.ingredientById[id] = ingredient;
        SaveIngredients();
    }

    public void DeleteIngredient(int id)
    {
        Ingredient ingredient = GetIngredientById(id);
        this.ingredients.Remove(ingredient);
        this.ingredientById.Remove(id);
        SaveIngredients();
    }
}