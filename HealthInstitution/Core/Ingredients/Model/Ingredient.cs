namespace HealthInstitution.Core.Ingredients.Model;

public class Ingredient
{
    public String name { get; set; }
    public Ingredient(string name)
    {
        this.name = name;
    }
}