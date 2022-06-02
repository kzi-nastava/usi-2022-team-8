using System.Text.Json.Serialization;

namespace HealthInstitution.Core.Ingredients.Model;

public class Ingredient
{
    public int Id { get; set; }
    public String Name { get; set; }

    [JsonConstructor]
    public Ingredient(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }
    public override string? ToString()
    {
        return this.Name;
    }
}