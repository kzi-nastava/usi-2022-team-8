using HealthInstitution.Core.Ingredients.Model;

namespace HealthInstitution.Core.Drugs.Model;

public class Drug
{
    public int Id { get; set; }    
    public String Name { get; set; }
    public DrugState State { get; set; }
    public List<Ingredient> Ingredients { get; set; }

    public Drug(int id, string name, DrugState state, List<Ingredient> ingredients)
    {
        this.Id = id;
        this.Name = name;
        this.State = state;
        this.Ingredients = ingredients;
    }

    public override string ToString()
    {
        return this.Name;
    }
}

public enum DrugState
{
    Created,
    Accepted,
    Rejected
}