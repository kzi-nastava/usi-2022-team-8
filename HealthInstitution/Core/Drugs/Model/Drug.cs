using HealthInstitution.Core.Ingredients.Model;

namespace HealthInstitution.Core.Drugs.Model;

public class Drug
{
    public int id { get; set; }    
    public String name { get; set; }
    public DrugState state { get; set; }
    public List<Ingredient> ingredients { get; set; }

    public Drug(int id, string name, DrugState state, List<Ingredient> ingredients)
    {
        this.id = id;
        this.name = name;
        this.state = state;
        this.ingredients = ingredients;
    }
}

public enum DrugState
{
    Created,
    Accepted,
    Rejected
}