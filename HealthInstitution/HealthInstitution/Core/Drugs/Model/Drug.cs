using HealthInstitution.Core.Ingredients.Model;

namespace HealthInstitution.Core.Drugs.Model;

public class Drug
{
    public String name { get; set; }
    public DrugState state { get; set; }
    public List<Ingredient> ingredients { get; set; }

    public Drug(string name, List<Ingredient> ingredients)
    {
        this.name = name;
        this.state = DrugState.Created;
        this.ingredients = ingredients;
    }
}

public enum DrugState
{
    Created,
    Accepted,
    Rejected
}