namespace HealthInstitution.Core.Ingredients.Model;

public class Ingredient
{
    public int id { get; set; }
    public String name { get; set; }

    public Ingredient(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}